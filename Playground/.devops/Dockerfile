#Official .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0

# Download and install the .NET SDK
RUN apt update && apt install -y python3 python3-pip \
	&& pip3 install --no-cache-dir --break-system-packages notebook \
	&& dotnet tool install -g --add-source "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json" Microsoft.dotnet-interactive

# Set environment variables
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV PATH="/usr/share/dotnet:$PATH"

# Set non-root user
ENV USER="user"
RUN useradd -M -u 1000 $USER
USER $USER 
ENV HOME="/home/$USER"
WORKDIR $HOME

# Install .NET Interactive
RUN dotnet tool install -g --add-source "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json" Microsoft.dotnet-interactive

# Set environment variables
ENV PATH="/home/$USER/.dotnet/tools:${PATH}"

# Edit .dotnet directory permissions for the nom-root user
RUN chown -R $USER:$USER $HOME/.dotnet

# Instalar o kernel do .NET para o Jupyter
RUN dotnet interactive jupyter install

EXPOSE 8888

CMD ["jupyter", "notebook", "--no-browser", "--ip=0.0.0.0"]