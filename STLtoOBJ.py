import bpy,os,shutil
filesHomeDirectory = "D:\\OVGU\\Thesis\\Data\\D3012\\threeDimage"
exportedDirectory = "\\Newfolder"
stls = 0
objs = 0
others = 0 
for file in os.listdir(filesHomeDirectory):
    if file.endswith(".stl"):
        stls += 1
        fileName = file.split(".")[0]
        filePathValue=filesHomeDirectory + "\\" + file
        obj = bpy.ops.import_mesh.stl(filepath=filePathValue)
        filePathValueDest=filesHomeDirectory + exportedDirectory + "\\"  + fileName + ".obj"
        bpy.ops.export_scene.obj(filepath=filePathValueDest,use_materials=False)
        allObjects = bpy.data.objects
        allObjects.remove(allObjects[str(fileName).replace('_', ' ')], do_unlink=True)
    elif file.endswith(".obj"):
        objs += 1
        filePathValue=filesHomeDirectory + "\\" + file
        filePathValueDest=filesHomeDirectory + exportedDirectory
        shutil.copy2(filePathValue,filePathValueDest)
    else:
        others += 1
        filename, file_extension = os.path.splitext(file)
        print(file)
print("\n")
print("Other Files - " + str(others))
print("OBJ Files - " + str(objs))