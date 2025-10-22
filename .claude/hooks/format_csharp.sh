#!/usr/bin/env bash
set -euo pipefail

INPUT="$(cat)"
FILE="$(printf '%s' "$INPUT" | jq -r '.tool_input.file_path // empty')"
[ -z "$FILE" ] && exit 0

case "$FILE" in
  *.cs)
    ROOT="${CLAUDE_PROJECT_DIR:-$PWD}"
    REL="${FILE#${ROOT}/}"  # 相対パス化
    # 変更ファイルのみ整形（高速・自動修正）
    dotnet format "FG.sln" --no-restore --include "$REL" >/dev/null || true
    echo "✓ dotnet format applied: $REL"
    ;;
esac

exit 0
