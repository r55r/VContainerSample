#!/usr/bin/env bash
set -euo pipefail

ROOT="${CLAUDE_PROJECT_DIR:-$PWD}"
REPORT="$ROOT/.claude/reports"
mkdir -p "$REPORT"

# .slnファイルを自動検出
SLN_FILE=$(find "$ROOT" -maxdepth 1 -name "*.sln" | head -n 1)
if [ -z "$SLN_FILE" ]; then
  echo "Error: No .sln file found in $ROOT"
  exit 0
fi

# 初期化
F_OK=1
I_ERR=0
I_WARN=0
R_CNT=0

# dotnet format チェック
if dotnet format "$SLN_FILE" --no-restore --verify-no-changes >/dev/null 2>&1; then
  F_OK=0
fi

# R# InspectCode（SARIF）
INSPECT="$REPORT/inspect.sarif"
if command -v jb >/dev/null 2>&1; then
  jb inspectcode "$SLN_FILE" -o="$INSPECT" >/dev/null 2>&1 || true

  # SARIF のレベル別件数を集計（jq）
  if command -v jq >/dev/null 2>&1 && [ -f "$INSPECT" ]; then
    JQ='[.runs[].results[]? .level] | group_by(.) | map({(.[0]):length}) | add // {}'
    READABLE="$(jq -r "$JQ" "$INSPECT" 2>/dev/null || echo '{}')"
    I_ERR="$(jq -r '.error // 0' <<<"$READABLE" 2>/dev/null || echo "0")"
    I_WARN="$(jq -r '.warning // 0' <<<"$READABLE" 2>/dev/null || echo "0")"
  fi
fi

# Roslynator（XML）
ROSLY="$REPORT/roslynator.xml"
if command -v roslynator >/dev/null 2>&1; then
  if [ -d "tools/analyzers" ]; then
    roslynator analyze "$SLN_FILE" --analyzer-assemblies tools/analyzers \
      --severity-level info --output "$ROSLY" --output-format xml >/dev/null 2>&1 || true
  else
    roslynator analyze "$SLN_FILE" \
      --severity-level info --output "$ROSLY" --output-format xml >/dev/null 2>&1 || true
  fi

  if [ -f "$ROSLY" ]; then
    R_CNT="$(grep -c '<Diagnostic' "$ROSLY" 2>/dev/null || echo "0")"
  fi
fi

# いずれかヒットで Stop をブロックし、Claude に次アクションを指示
if [ "$F_OK" -ne 0 ] || [ "$I_ERR" -gt 0 ] || [ "$I_WARN" -gt 0 ] || [ "$R_CNT" -gt 0 ]; then
  REASON="Quality gate 未達。dotnet format: $( [ $F_OK -eq 0 ] && echo OK || echo NG ); InspectCode error:${I_ERR} warn:${I_WARN}; Roslynator:${R_CNT}。レポートを確認: InspectCode=$INSPECT ; Roslynator=$ROSLY"
  cat <<JSON
{
  "decision": "block",
  "reason": "$REASON"
}
JSON
else
  echo "Quality gate passed."
fi
