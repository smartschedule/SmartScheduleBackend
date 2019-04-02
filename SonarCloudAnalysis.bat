dotnet tool install --global dotnet-sonarscanner

dotnet sonarscanner begin /k:"adambajguz_SmartScheduleBackend" /o:"adambajguz-github" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.login="706c2a89d52059f9c0c70ffb2741e9263bcf39a4"
dotnet build SmartSchedule.sln
dotnet sonarscanner end /d:sonar.login="706c2a89d52059f9c0c70ffb2741e9263bcf39a4"

PAUSE