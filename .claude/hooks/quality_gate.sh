#!/usr/bin/env bash
set -euo pipefail

ROOT="${CLAUDE_PROJECT_DIR:-$PWD}"
REPORT="$ROOT/.claude/reports"
mkdir -p "$REPORT"

F_OK=1
if dotnet format "FG.sln" --no-restore --verify-no-changes >/dev/null 2>&1; then
  F_OK=0
fi

# R# InspectCode（SARIF）
INSPECT="$REPORT/inspect.sarif"
jb inspectcode "FG.sln" -o="$INSPECT" >/dev/null || true

# SARIF のレベル別件数を集計（jq）
JQ='[.runs[].results[]? .level] | group_by(.) | map({(.[0]):length}) | add // {}'
READABLE="$(jq -r "$JQ" "$INSPECT" 2>/dev/null || echo '{}')"
I_ERR="$(jq -r '.error // 0' <<<"$READABLE")"
I_WARN="$(jq -r '.warning // 0' <<<"$READABLE")"

# Roslynator（XML）
ROSLY="$REPORT/roslynator.xml"
roslynator analyze "FG.sln" --analyzer-assemblies tools/analyzers \
  --severity-level info --output "$ROSLY" --output-format xml >/dev/null || true
R_CNT="$(grep -c '<Diagnostic' "$ROSLY" || true)"

# いずれかヒットで Stop をブロックし、Claude に次アクションを指示
if [ "$F_OK" -ne 0 ] || [ "$I_ERR" -gt 0 ] || [ "$I_WARN" -gt 0 ] || [ "${R_CNT:-0}" -gt 0 ]; then
  REASON="Quality gate 未達。dotnet format: $( [ $F_OK -eq 0 ] && echo OK || echo NG ); InspectCode error:$I_ERR warn:$I_WARN; Roslynator:$R_CNT。レポートを確認して修正してから再実行してください。"
  cat <<JSON
{
  "decision": "block",
  "reason": "$REASON",
  "hookSpecificOutput": {
    "hookEventName": "Stop",
    "additionalContext": "InspectCode: $INSPECT ; Roslynator: $ROSLY"
  }
}
JSON
else
  echo "Quality gate passed."
fi
