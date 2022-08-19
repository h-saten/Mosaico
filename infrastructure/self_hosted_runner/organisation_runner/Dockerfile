FROM ubuntu:20.04
ARG DEBIAN_FRONTEND=noninteractive
ENV TZ=Europe/Warsaw
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
ENV GITHUB_PAT ""
ENV GITHUB_TOKEN ""
ENV GITHUB_OWNER ""
ENV GITHUB_REPOSITORY ""
ENV RUNNER_WORKDIR "_work"
ENV RUNNER_LABELS ""
ENV ADDITIONAL_PACKAGES ""
RUN DEBIAN_FRONTEND=noninteractive apt-get update \
    && apt-get install -y \
        curl \
        sudo \
        git \
        jq \
        iputils-ping \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/* \
    && useradd -m -d /home/github -s /bin/bash -g root -G sudo -u 1001 github
    
RUN mkdir -p /home/github

WORKDIR home/github

RUN GITHUB_RUNNER_VERSION=$(curl --silent "https://api.github.com/repos/actions/runner/releases/latest" | jq -r '.tag_name[1:]') \
    && curl -Ls https://github.com/actions/runner/releases/download/v${GITHUB_RUNNER_VERSION}/actions-runner-linux-x64-${GITHUB_RUNNER_VERSION}.tar.gz | tar xz \
    && ./bin/installdependencies.sh
COPY --chown=github:root start.sh /home/github/
RUN chmod u+x /home/github/start.sh 

RUN apt-get update -y && apt-get install -y build-essential \
    && apt-get install -y unzip

RUN curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash
RUN apt-get update -y
RUN update-ca-certificates
RUN az upgrade -y
#UPGRADE TO AZ CLI 2.34.1
# ENTRYPOINT ["./config.sh --url https://github.com/sapiency-io --token AR2FPJ3T25SHRXEH2WMF3B3CYMBSS --labels self-hosted,Linux,X64,dev"]
USER github
ENTRYPOINT ["./start.sh"]
# REMEMBER TO REMOVE DOCKER BUILDER CACHE IF SOMETHING ISNT INSTALLED PROPERLY IN IMAGE!! (docker builder prune/docker system prune -a)