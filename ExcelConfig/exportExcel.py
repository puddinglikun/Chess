import os
import pandas as pd
import json
    
class ExportExcel:
    excelPath = "./excel/0设置表格导出.xlsx"
    excelInfoList = []

    def __init__(self):
        print("开始转化配表文件")
        df = pd.read_excel(self.excelPath)
        # 获取最大行 最大列
        nrows = df.shape[0]
        ncols = df.columns.size

        #遍历逐行逐列 并将配表信息保存起来
        for iRow in range(nrows):
            excelName = df.iloc[iRow, 0]
            jsonName = df.iloc[iRow, 1]
            print("读取：" + excelName + " 路径名：" + jsonName)
            info = ExcelInfo(excelName, jsonName)
            self.excelInfoList.append(info)

        pass
    
    def ReadExcelList(self):
        # 读取excel表并转化文件
        configMainString = "using System.Collections.Generic;\nusing Utils;\n\nnamespace Config\n{\n\tpublic class ConfigMgr : Singleton<ConfigMgr>\n\t{\n"
        configInitString = ""

        for excelInfo in self.excelInfoList:
            df = pd.read_excel(excelInfo.excelName)
            # 获取最大行 最大列
            nrows = df.shape[0]
            ncols = df.columns.size
            
            # 若行数小于等于2 则没有足够的数据 直接遍历下一个表
            if(nrows <= 2):
                print("表格：" + excelInfo.excelName + "内容不够 请检查")
                continue

            # 记录表头数据格式 以及数据结构
            dataTitleList = []
            dataTypeList = []
            for iCol in range(ncols):
                dataTitleList.append(df.iloc[0, iCol])
                dataTypeList.append(df.iloc[1, iCol])
            # 从第2行开始遍历添加
            excelList = []
            for iRow in range(2, nrows):
                # 表格字典
                excelDict = {}
                for iCol in range(ncols):
                    excelDict[dataTitleList[iCol]] = df.iloc[iRow,iCol]
                
                excelList.append(excelDict)

            # 转化成json格式
            jsonData = json.dumps(excelList, ensure_ascii=False)
            # print(jsonData)
            print("输出json文件：" + excelInfo.exportJsonName)

            try:
                with open(excelInfo.exportJsonName, "w", encoding="utf-8") as f:
                    f.write(jsonData)
            except e:
                print("json文件转化失败 " + excelInfo.exportJsonName)
            
            # 主文件写入
            tmpClassName = "Config" + excelInfo.fileName
            tmpPathName = "path" + excelInfo.fileName
            configMainString += "\t\t/// <summary>\n\t\t///" + excelInfo.excel + "\n\t\t/// </summary>\n"
            configMainString += "\t\tpublic List<" + excelInfo.fileName +"> " + tmpClassName + ";\n"
            # 主文件 初始化数据
            configInitString += "\t\t\tstring "+ tmpPathName +" = \"Assets/Script/ConfigExcel/Json/" + excelInfo.fileName + ".json\";\n"
            configInitString += "\t\t\t" + tmpClassName + " = ConfigUtil.ReadConfig<List<"+excelInfo.fileName+">>("+ tmpPathName +");\n\n"

            # 写成cs文件
            self.CreateCShellFile(dataTitleList, dataTypeList, excelInfo)
        
        configMainString += "\n\t\tpublic void InitConfig()\n\t\t{\n" + configInitString + "\t\t}\n\t}\n}\n"
        configFilePath = os.path.join("../Assets/Script/ConfigExcel/ConfigMgr.cs")
        try:
            with open(configFilePath, "w", encoding="utf-8") as f:
                f.write(configMainString)
        except e:
            print("主文件转化失败 " + configFilePath)
        pass


    def CreateCShellFile(self, titleList, typeList, info):
        # 创建cs文件
        fileString = "namespace Config\n{\n\tpublic class " + info.fileName + "\n\t{\n"
        for index in range(len(titleList)):
            keyName = titleList[index]
            typeName = typeList[index]
            tmpTypeName = self.ChangeType(typeName)

            line = "\t\tpublic " + tmpTypeName + " " + keyName + " { get; set; }\n"
            fileString += line
        fileString += "\t}\n}"

        print("输出cs文件：" + info.exportCShellName)
        try:
            with open(info.exportCShellName, "w", encoding="utf-8") as f:
                f.write(fileString)
        except e:
            print("输出cs文件转化失败 " + excelInfo.exportJsonName)
        
    

    def ChangeType(self, type):
        # 修改类型名
        typeName = "int"
        if(type == "int32"):
            typeName = "int"
        if(type == "string"):
            typeName = "string"
        if(type == "float"):
            typeName = "float"
        return typeName


class ExcelInfo:
    excelName = ""
    excel = ""
    fileName = ""
    exportJsonName = ""
    exportCShellName = ""
    def __init__(self, excel, export):
        self.fileName = export
        self.excel = excel
        self.excelName = os.path.join(excelDir, excel + ".xlsx")
        self.exportJsonName = os.path.join(jsonDir, export + ".json") 
        self.exportCShellName = os.path.join(cshellDir, export + ".cs")
        pass
    

if __name__ == "__main__":
    # 配表的路径
    excelDir = "./excel"
    # 生成的json路径
    jsonDir = "../Assets/Script/ConfigExcel/Json"
    # 生成的cs文件路径
    cshellDir = "../Assets/Script/ConfigExcel/ConfigClass"
    exportUtil = ExportExcel()
    exportUtil.ReadExcelList()
    pass