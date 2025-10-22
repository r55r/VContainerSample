#!/usr/bin/env bash
set -euo pipefail

INPUT="$(cat)"
FILE="$(printf '%s' "$INPUT" | jq -r '.tool_input.file_path // empty')"
[ -z "$FILE" ] && exit 0
case "$FILE" in
  *.cs)
    ROOT="${CLAUDE_PROJECT_DIR:-$PWD}"
    REL="${FILE#${ROOT}/}"
    OUT_DIR="$ROOT/.claude/reports/roslynator-onwrite"
    mkdir -p "$OUT_DIR"
    OUT="$OUT_DIR/$(basename "$REL").xml"

    # 変更ファイルに絞って解析（Warning以上のみ）
    roslynator analyze "FG.sln" \
      --analyzer-assemblies tools/analyzers \
      --include "$REL" \
      --severity-level warning \
      --output "$OUT" --output-format xml || true

    # 指摘検出で Claude に“続行禁止 + フィードバック”を返す
    COUNT="$(grep -c '<Diagnostic' "$OUT" || true)"
    if [ "${COUNT:-0}" -gt 0 ]; then
      cat <<JSON
{
  "decision": "block",
  "reason": "Roslynator が警告/エラーを $COUNT 件検出しました。レポート: $OUT を見て修正してください。",
  "hookSpecificOutput": {
    "hookEventName": "PostToolUse",
    "additionalContext": "Roslynator report: $OUT"
  }
}
JSON
      exit 0
    fi
    ;;
esac

exit 0
