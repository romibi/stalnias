#!/bin/bash
project="stalnias"
DATE=`date +%y.%m.%d`
versionName="v.a.${DATE}"
imageName="${project}_${versionName}_${TRAVIS_BUILD_NUMBER}_${TRAVIS_COMMIT::8}"

mkdir ${versionName}

workingDir=`pwd`

if [ -d Build/windows/ ]; then
	cd Build/windows/
	zip -r -X ${workingDir}/${versionName}/${imageName}_windows.zip .
	cd ${workingDir}
fi

if [ -d Build/osx/ ]; then
	cd Build/osx/
	zip -r -X ${workingDir}/${versionName}/${imageName}_osx.zip .
	cd ${workingDir}
fi

if [ -d Build/linux/ ]; then
	cd Build/linux/
	zip -r -X ${workingDir}/${versionName}/${imageName}_linux.zip .
	cd ${workingDir}
fi

#if [ -d Build/webgl/ ]; then
#	cd Build/webgl/
#	zip -r -X ${workingDir}/${versionName}/${imageName}_webgl.zip .
#	cd ${workingDir}
#fi

echo "Uploading packages to server..."
scp -o StrictHostKeyChecking=no -o UserKnownHostsFile=/dev/null -q -r ${versionName} ${deploy_host}:${deploy_root_dir}/${versionName}