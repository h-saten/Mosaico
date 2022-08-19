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
COPY --chown=github:root entrypoint.sh runsvc.sh /home/github/
RUN chmod u+x /home/github/entrypoint.sh /home/github/runsvc.sh
# MAKE is required to run tests for ganache on self hosted runner
RUN apt-get update -y && apt-get install -y build-essential \
    && apt-get install -y unzip
# RUN apt-get install -y ca-certificates curl apt-transport-https lsb-release gnupg \
#     && curl -sL https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor | sudo tee /etc/apt/trusted.gpg.d/microsoft.gpg > /dev/null\
#     && AZ_REPO=$(lsb_release -cs)\
#     && echo "deb [arch=amd64] https://packages.microsoft.com/repos/azure-cli/ $AZ_REPO main" | sudo tee /etc/apt/sources.list.d/azure-cli.list\
#     && apt-get update
    # && apt-get install -y azure-cli
RUN curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash
RUN apt-get update -y
RUN update-ca-certificates
RUN az upgrade -y
#UPGRADE TO AZ CLI 2.34.1
ENTRYPOINT ["/home/github/entrypoint.sh"]
USER github
#IT CAN RUN ON AZ CLI 2.34.1 WHEN YOU RUN SELF HOSTED AS ROOT, WHY? I DONT KNOW
# currently it runs as user github, more secure, but runs on az cli 2.33.1
# REMEMBER TO REMOVE DOCKER BUILDER CACHE IF SOMETHING ISNT INSTALLED PROPERLY IN IMAGE!! (docker builder prune/docker system prune -a)