#!/bin/bash
project="stalnias"
DATE=`date +%y.%m.%d`
versionName="a.${DATE}"
imageName="${project}_${versionName}_${TRAVIS_BUILD_NUMBER}_${TRAVIS_COMMIT::8}"

# Prepare
mkdir ${versionName}

workingDir=`pwd`

if [ -d Build/windows/ ]; then
	cd Build/windows/
	zip -r -X ${workingDir}/${versionName}/${imageName}_windows.zip .
	cd ${workingDir}
fi

if [ -d Build/osx/ ]; then
	PACKAGESIZE=$(echo "$(du -ms Build/osx/ | cut -f1) * 1.1" | bc)
	hdiutil create -srcfolder Build/osx -format UDBZ -size ${PACKAGESIZE}M -volname "Stalnias ${versionName}" "${workingDir}/${versionName}/${imageName}_osx.dmg" >/dev/null
fi

if [ -d Build/linux/ ]; then
	cd Build/linux/
	zip -r -X ${workingDir}/${versionName}/${imageName}_linux.zip .
	cd ${workingDir}
fi

if [ -d Build/webgl/ ]; then
	cd Build/webgl/
	zip -r -X ${workingDir}/${versionName}/${imageName}_webgl.zip .
	cd ${workingDir}
fi

if [ -d Build/android/ ]; then
	cp Build/android/${project}.${versionName}.apk ${workingDir}/${versionName}/${project}.${versionName}.apk
fi


# Upload
if [ -d Build/webgl/ ]; then
	echo "Updating online WebGL page"
	scp -o StrictHostKeyChecking=no -o UserKnownHostsFile=/dev/null -q -r Build/webgl/$project/* ${deploy_host}:${deploy_webgl_dir}	
fi

echo "Uploading packages to server..."
scp -o StrictHostKeyChecking=no -o UserKnownHostsFile=/dev/null -q -r ${versionName} ${deploy_host}:${deploy_root_dir}/${versionName}