FROM alpine:3.19

# Instalar dependências
RUN apk add --update \
	--no-cache \
	icu-libs \
	krb5-libs \
	libgcc \
	libintl \
	libssl1.1 \
	libstdc++ \
	zlib \
	wget \
	ca-certificates \
	bash \
	python3 \
	py3-pip \
	gcc \
	python3-dev \
	musl-dev \
	linux-headers

RUN pip3 install --no-cache-dir notebook

# Download e instalação do SDK do .NET
RUN wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh && \
	chmod +x dotnet-install.sh && \
	./dotnet-install.sh --version latest --install-dir /usr/share/dotnet && \
	rm dotnet-install.sh

# Adicionar o diretório de ferramentas do .NET ao PATH
ENV PATH="/usr/share/dotnet:$PATH"

# Set non-root user
ENV USER="user"
RUN adduser -D -u 1000 $USER 
USER $USER 
ENV HOME="/home/$USER"
WORKDIR $HOME

# Instalar .NET Interactive
RUN dotnet tool install -g --add-source "https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json" Microsoft.dotnet-interactive

# Configurar variáveis de ambiente
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV PATH="/home/$USER/.dotnet/tools:${PATH}"

# Alterar permissões do diretório .dotnet para o usuário não root
RUN chown -R $USER:$USER $HOME/.dotnet


# Instalar o kernel do .NET para o Jupyter
RUN dotnet interactive jupyter install

# Expor a porta 8888 para o Jupyter Notebook
EXPOSE 8888

CMD ["jupyter", "notebook", "--no-browser", "--ip=0.0.0.0"]