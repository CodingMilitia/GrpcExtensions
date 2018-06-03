rmdir /S /Q Generated
mkdir Generated
copy %UserProfile%\.nuget\packages\grpc.tools\1.10.0\tools\windows_x64\protoc.exe .
copy %UserProfile%\.nuget\packages\grpc.tools\1.10.0\tools\windows_x64\grpc_csharp_plugin.exe .
protoc service.proto --csharp_out ./Generated/. --grpc_out ./Generated/. --plugin=protoc-gen-grpc=grpc_csharp_plugin.exe
del protoc.exe
del grpc_csharp_plugin.exe