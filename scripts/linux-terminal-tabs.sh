#!/bin/sh
currentPath=$(pwd)

cd "$currentPath" && cd .. && cd docker_compose && docker-compose -f docker-compose.linux.yml up -d && dcSuccess=1

#Execute next steps only if docker compose run succesfully
if [ $dcSuccess == 1 ];
then 
gnome-terminal --tab --title="Core Backend"     -e "bash -c 'cd $currentPath && cd .. && cd src/Mosaico.Core.Service && dotnet run';bash"
gnome-terminal --tab --title="Core UI"          -e "bash -c 'handleCtrlC() { exec bash; }; trap handleCtrlC INT; cd $currentPath; cd ..; cd frontend/mosaico-web-ui; npm install; npm start';bash"
gnome-terminal --tab --title="Identity Backend" -e "bash -c 'cd $currentPath && cd .. && cd src/Mosaico.Identity && dotnet run';bash"
gnome-terminal --tab --title="Identity UI"      -e "bash -c 'handleCtrlC() { exec bash; }; trap handleCtrlC INT; cd $currentPath; cd ..; cd frontend/mosaico-id-ui; pwd; npm install; npm start';bash"
fi
