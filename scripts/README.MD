# Prerequisites
If you want to run all solutions locally and outside any IDE and you do not plan to debug the code, you could use scripts from this folder to do it.
First, run the following command to enable PowerShell script execution on your machine:
`Set-Executionpolicy -Scope CurrentUser -ExecutionPolicy UnRestricted`

# Installation
1) Install Tye using `./install-tye.ps1`.
2) Install required npm packages using `./install-npm-packages.ps1`

# Run
Run `./run-all-tye.ps1` in to start required backend and frontend services.
** Please note: in case of running the project LOCALLY, after each git pull you need to run `./install-npm-packages.ps1` to ensure new dependencies (if any) applied. Then use mentioned above `./run-all-tye.ps1` for the correct application work

# Dashboard
To access dashboard and logs of each application, navigate to http://127.0.0.1:8000/ when tye is running.

# Documentation

Read more about configuration: https://github.com/dotnet/tye/blob/main/docs/reference/schema.md 

# Scripts

## install-npm-packages
Install NPM packages (dependencies) in core and identity frontend projects

## install-tye
Installs dotnet tool Tye to run multiple projects at a time

## recreate-databases
Deletes Mosaico and mosaicoid databases from SQL server hosted at localhost and creates empty databases

## run-all-tye
Runs all required solutions in watch mode: identity backend, core backend, core frontend, identity frontend

## run-tye-frontend-identity
Runs following solutions in a watch mode: identity backend, core frontend, identity frontend

## run-data-seed
Runs a command line tool to generate 300 users, 100 projects, 100 tokens, 100 companies, 100 transactions per project.
