# 🚀 最好用的 Revit 进度条封装组件！

![常规用法](HD.gif) ![取消进度条用法](Cancel-HD.gif)

---

## 🧩 项目介绍

本项目是 **专为 Revit 二次开发设计的进度条封装组件**，  
基于原开源项目 [ricaun.Revit.UI.StatusBar](https://github.com/ricaun-io/ricaun.Revit.UI.StatusBar) 进行拓展开发。  

由于原作者未计划支持 **Revit 2018 及以下版本**，且不支持修改进度条样式或默认文本，  
为适应 **国内 Revit 二开现状（仍普遍使用 2020 以下版本，且要求中文 UI）**，  
本项目应运而生 🚀

---

## ✨ 功能新增

| 功能项 | 描述 |
|--------|------|
| ✅ **支持旧版本 Revit** | 新增支持 **Revit 2011 - Revit 2018** 全系列版本 |
| 🎯 **界面优化** | 进度条位置调整至 **Ribbon 下方**，更贴合 Revit 操作体验 |
| 🧰 **可定制样式** | 支持传入 `Options` 参数来自定义进度条样式比如：颜色、文本、尺寸等(目前仅支持取消按钮Button的Content） |

---

## 🧱 版本支持

| Revit 版本 | 支持情况 |
|-------------|-----------|
| 2011 | ✅ |
| 2012 | ✅ |
| 2013 | ✅ |
| 2014 | ✅ |
| 2015 | ✅ |
| 2016 | ✅ |
| 2017 | ✅ |
| 2018 | ✅ |
| 2019 | ✅ |
| 2020 | ✅ |
| 2021 | ✅ |
| 2022 | ✅ |
| 2023 | ✅ |
| 2024 | ✅ |
| 2025 | ✅ |
| 2026 | ✅ |

---

## 🪄 使用方法

### 1️⃣ 基础用法 —— 遍历集合元素

```csharp
// 示例：批量处理墙元素
var walls = new FilteredElementCollector(doc)
    .OfCategory(BuiltInCategory.OST_Walls)
    .WhereElementIsNotElementType()
    .Cast<Wall>();

ProgressBarExUtils.Run(
    elements: walls,
    loopAction: wall =>
    {
        wall.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS)
           ?.Set("已处理");
    }
);

```

### 2. 使用整数计数循环
```csharp
int count = 50;
ProgressBarExUtils.Run(
    count: count,
    loopAction: i =>
    {
        // i 从 0 到 count-1
        Task.Delay(50).Wait(); // 模拟耗时操作
    }
);
);

```
### 3.可取消的事务内循环（Transaction）
```csharp

  var walls = new FilteredElementCollector(doc)
      .OfCategory(BuiltInCategory.OST_Walls)
      .WhereElementIsNotElementType()
      .Cast<Wall>();
  using (var tx = new Transaction(doc, "批量修改"))
  {
      tx.Start();
      ProgressBarExUtils.RunCancelable(
          transaction: tx,
          sources: walls,
          loopAction: wall =>
          {
              Thread.Sleep(100);
              wall.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS)
                  ?.Set("批量处理完成");
          }
      );
      if (tx.GetStatus() == TransactionStatus.Started)
      {
          tx.Commit();
      }
  }

```

### 4. 可取消的事务组内循环（TransactionGroup）
```csharp

var walls = new FilteredElementCollector(doc)
    .OfCategory(BuiltInCategory.OST_Walls)
    .WhereElementIsNotElementType()
    .Cast<Wall>();

using (var tg = new TransactionGroup(doc, "事务组批量处理"))
{
    tg.Start();
    ProgressBarExUtils.RunCancelable(
        transactionGroup: tg,
        sources: walls,
        loopAction: wall =>
        {
          wall.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS)
                   ?.Set("事务组处理完成");
        }
    );
    tg.Assimilate(); // 合并事务组
}

```