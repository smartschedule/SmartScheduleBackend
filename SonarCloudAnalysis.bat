dotnet tool install --global dotnet-sonarscanner

dotnet sonarscanner begin /k:"adambajguz_SmartScheduleBackend" /o:"adambajguz-github" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.login="706c2a89d52059f9c0c70ffb2741e9263bcf39a4" /d:sonar.sourceEncoding="UTF-8" /d:sonar.language="cs" /d:sonar.exclusions="**/bin/**/*,**/obj/**/*" /d:sonar.cs.opencover.reportsPaths="%cd%\lcov.opencover.xml"
dotnet restore
dotnet build
dotnet test ./SmartSchedule.Test/SmartSchedule.Test.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=\"opencover,lcov\" /p:CoverletOutput=../lcov /p:Exclude="[xunit*]*"
dotnet sonarscanner end /d:sonar.login="706c2a89d52059f9c0c70ffb2741e9263bcf39a4"

PAUSE