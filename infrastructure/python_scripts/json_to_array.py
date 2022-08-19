#used for huginn rss feeds, convert data from json to set up proper array so huginn gets it
import os

huginn_array = []

print(os.getcwd())

packages = open("./id_ui_packages.txt","r") #manually enter txt data
baseline_url1 = "https://libraries.io/npm/" #could be any other library
baseline_url2 = "/versions.atom"

for line in packages:
    line = line.split(":")[0].strip("\"")
    final_url=baseline_url1+line+baseline_url2
    huginn_array.append(final_url)

# print(huginn_array)
final_array = "['https://libraries.io/npm/@angular/animations/versions.atom', 'https://libraries.io/npm/@angular/cdk/versions.atom', 'https://libraries.io/npm/@angular/common/versions.atom', 'https://libraries.io/npm/@angular/compiler/versions.atom', 'https://libraries.io/npm/@angular/core/versions.atom', 'https://libraries.io/npm/@angular/forms/versions.atom', 'https://libraries.io/npm/@angular/localize/versions.atom', 'https://libraries.io/npm/@angular/material/versions.atom', 'https://libraries.io/npm/@angular/platform-browser/versions.atom', 'https://libraries.io/npm/@angular/platform-browser-dynamic/versions.atom', 'https://libraries.io/npm/@angular/router/versions.atom', 'https://libraries.io/npm/@ngx-pwa/local-storage/versions.atom', 'https://libraries.io/npm/@ngx-translate/core/versions.atom', 'https://libraries.io/npm/@ngx-translate/http-loader/versions.atom', 'https://libraries.io/npm/bootstrap/versions.atom', 'https://libraries.io/npm/core-js/versions.atom', 'https://libraries.io/npm/ng-recaptcha/versions.atom', 'https://libraries.io/npm/ngx-cookie-service/versions.atom', 'https://libraries.io/npm/rxjs/versions.atom', 'https://libraries.io/npm/subsink/versions.atom', 'https://libraries.io/npm/tslib/versions.atom', 'https://libraries.io/npm/zone.js/versions.atom', 'https://libraries.io/npm/@angular-devkit/build-angular/versions.atom', 'https://libraries.io/npm/@angular/cli/versions.atom', 'https://libraries.io/npm/@angular/compiler-cli/versions.atom', 'https://libraries.io/npm/@angular/language-service/versions.atom', 'https://libraries.io/npm/@fortawesome/fontawesome-free/versions.atom', 'https://libraries.io/npm/@types/jasmine/versions.atom', 'https://libraries.io/npm/@types/jasminewd2/versions.atom', 'https://libraries.io/npm/@types/node/versions.atom', 'https://libraries.io/npm/codelyzer/versions.atom', 'https://libraries.io/npm/jasmine-core/versions.atom', 'https://libraries.io/npm/jasmine-spec-reporter/versions.atom', 'https://libraries.io/npm/karma/versions.atom', 'https://libraries.io/npm/karma-chrome-launcher/versions.atom', 'https://libraries.io/npm/karma-coverage-istanbul-reporter/versions.atom', 'https://libraries.io/npm/karma-jasmine/versions.atom', 'https://libraries.io/npm/karma-jasmine-html-reporter/versions.atom', 'https://libraries.io/npm/protractor/versions.atom', 'https://libraries.io/npm/ts-node/versions.atom', 'https://libraries.io/npm/tslint/versions.atom', 'https://libraries.io/npm/typescript/versions.atom']"
#manually paste final array from debug output to final_array variable^ (im lazy)
final_array = final_array.replace("'","\"")
print(final_array)