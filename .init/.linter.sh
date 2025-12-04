#!/bin/bash
cd /home/kavia/workspace/code-generation/securechat-messenger-219351-219360/whatsapp_backend
dotnet build --no-restore -v quiet -nologo -consoleloggerparameters:NoSummary /p:TreatWarningsAsErrors=false
LINT_EXIT_CODE=$?
if [ $LINT_EXIT_CODE -ne 0 ]; then
  exit 1
fi

