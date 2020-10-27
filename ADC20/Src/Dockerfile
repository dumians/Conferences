FROM alpine:latest as adc20_build

LABEL description="Build Navive container - Adcpp20"

#prepare linux image
RUN apk update && apk add --no-cache \ 
    autoconf build-base binutils curl file gcc g++ git libgcc libtool bash-completion vim linux-headers make musl-dev ninja tar unzip wget \
    && echo '. /usr/share/bash-completion/bash_completion && set -o vi' >> /root/.bashrc \
    && echo 'set hlsearch' >> /root/.vimrc \
    && ln -s /usr/local /opt/sysroot

FROM adc20_build as runtime

LABEL description="Run container - Adcpp20 runtime"

RUN apk update && apk add --no-cache \ 
   cmake ninja libstdc++ gdb openssh rsync zip gcc g++ git libgcc libtool linux-headers make curl unzip tar

#install vcpkg
RUN git clone https://github.com/Microsoft/vcpkg.git /opt/vcpkg

WORKDIR /opt/vcpkg

RUN ./bootstrap-vcpkg.sh -useSystemBinaries && ./vcpkg integrate install && ./vcpkg integrate bash && echo 'export PATH=$PATH:/opt/vcpkg' >>~/.bashrc
COPY x64-linux-musl.cmake /opt/vcpkg/triplets/

#conan
RUN git clone https://github.com/conan-community/conan-openssl /opt/conan
#WORKDIR /opt/conan
#RUN conan create . user/channel

#install BOOST
#RUN VCPKG_FORCE_SYSTEM_BINARIES=1 ./vcpkg install boost-asio boost-filesystem fmt http-parser opencv restinio
#RUN conan install Boost

#Build src's
COPY ./src /src
WORKDIR /src

# Use Clang to compile the Test.cpp source file
#RUN clang++ -stdlib=libc++ -lc++abi main.cpp

# Use GCC to compile the main.cpp source file
RUN g++ -o Test main.cpp
 
# Run the output program from the previous step
CMD ["./Test"]

RUN echo 'PermitRootLogin yes' >> /etc/ssh/sshd_config && \
    echo 'PermitEmptyPasswords yes' >> /etc/ssh/sshd_config && \
    echo 'PasswordAuthentication yes' >> /etc/ssh/sshd_config && \
    ssh-keygen -A

EXPOSE 22
#CMD ["/usr/sbin/sshd", "-D"]
