![Version](https://img.shields.io/badge/Support-Revit%202011~2026-blueviolet)
![License](https://img.shields.io/badge/license-MIT-green)

# 📦 Repository Information

**NuGet:** [https://www.nuget.org/packages/Su.Revit.UI.StatusBarEx](https://www.nuget.org/packages/Su.Revit.UI.StatusBarEx)

**Gitee:** [https://gitee.com/SususuChang/status-bar-ex](https://gitee.com/SususuChang/status-bar-ex)

**GitHub:** [https://github.com/SususuChang/status-bar-ex](https://github.com/ViewSuSu/Su.Revit.UI.StatusBarEx)

---

# 🚀 The Best Revit Progress Bar Component!

![Regular Usage](HD.gif) ![Cancel Progress Bar Usage](Cancel-HD.gif)

---

## 🧩 Project Introduction

This project is **a progress bar encapsulation component specifically designed for Revit secondary development**,
based on the original open-source project [ricaun.Revit.UI.StatusBar](https://github.com/ricaun-io/ricaun.Revit.UI.StatusBar) with extended development.

Since the original author did not plan to support **Revit 2018 and earlier versions**, and did not support modifying progress bar styles or default text,
to adapt to **the current situation of Revit secondary development in China (where versions below 2020 are still widely used, and Chinese UI is required)**,
this project was born 🚀

---

## ✨ New Features

| Feature | Description |
| :--- | :--- |
| ✅ **Support for Older Revit Versions** | Added support for **Revit 2011 - Revit 2026** full series |
| 🎯 **UI Optimization** | Progress bar position adjusted to **below the Ribbon**, more integrated with Revit operation experience |
| 🧰 **Customizable Styles** | Supports passing `Options` parameters to customize progress bar styles such as: color, text, size, etc. (currently only supports cancel button Content) |
| 🔄 **Cancel Operation Support** | Supports canceling execution during long operations, improving user experience |
| 🌐 **Multi-platform Support** | Supports both Gitee and GitHub code hosting |

---

## 🧩 Installation Methods

### Method 1: Package Manager Console

```powershell
# Choose the corresponding package according to your Revit version
Install-Package Su.Revit.UI.StatusBarEx.2018 -Version 1.0.0
Install-Package Su.Revit.UI.StatusBarEx.2019 -Version 1.0.0
Install-Package Su.Revit.UI.StatusBarEx.2020 -Version 1.0.0
# More versions please check NuGet page
```

### Method 2: .NET CLI

```bash
dotnet add package Su.Revit.UI.StatusBarEx.2018 --version 1.0.0
dotnet add package Su.Revit.UI.StatusBarEx.2019 --version 1.0.0
dotnet add package Su.Revit.UI.StatusBarEx.2020 --version 1.0.0
```

### Method 3: Visual Studio NuGet Package Manager

```
1. Right-click project → Manage NuGet Packages
2. Search for: Su.Revit.UI.StatusBarEx in Browse tab
3. Install the package suitable for your Revit version
```

---

## 🪄 Usage Methods

### 1️⃣ Basic Usage - Iterating Through Collection Elements

```csharp
// Example: Batch processing wall elements
var walls = new FilteredElementCollector(doc)
    .OfCategory(BuiltInCategory.OST_Walls)
    .WhereElementIsNotElementType()
    .Cast<Wall>();

ProgressBarExUtils.Run(
    elements: walls,
    loopAction: wall =>
    {
        wall.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS)
           ?.Set("Processed");
    }
);
```

---

### 2️⃣ Using Integer Count Loop

```csharp
int count = 50;
ProgressBarExUtils.Run(
    count: count,
    loopAction: i =>
    {
        // i from 0 to count-1
        Task.Delay(50).Wait(); // Simulate time-consuming operation
    }
);
```

---

### 3️⃣ Cancelable Transaction Loop

```csharp
var walls = new FilteredElementCollector(doc)
    .OfCategory(BuiltInCategory.OST_Walls)
    .WhereElementIsNotElementType()
    .Cast<Wall>();

using (var tx = new Transaction(doc, "Batch Modification"))
{
    tx.Start();
    ProgressBarExUtils.RunCancelable(
        transaction: tx,
        sources: walls,
        loopAction: wall =>
        {
            Thread.Sleep(100);
            wall.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS)
                ?.Set("Batch Processing Completed");
        }
    );
    if (tx.GetStatus() == TransactionStatus.Started)
    {
        tx.Commit();
    }
}
```

---

### 4️⃣ Cancelable Transaction Group Loop

```csharp
var walls = new FilteredElementCollector(doc)
    .OfCategory(BuiltInCategory.OST_Walls)
    .WhereElementIsNotElementType()
    .Cast<Wall>();

using (var tg = new TransactionGroup(doc, "Transaction Group Batch Processing"))
{
    tg.Start();
    ProgressBarExUtils.RunCancelable(
        transactionGroup: tg,
        sources: walls,
        loopAction: wall =>
    {
        wall.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS)
            ?.Set("Transaction Group Processing Completed");
    }
    );
    tg.Assimilate(); // Merge transaction group
}
```

---

### 5️⃣ Custom Options Usage

```csharp
var options = new ProgressBarOptions
{
    CancelButtonContent = "Cancel Operation",
    // More custom options...
};

ProgressBarExUtils.Run(
    elements: walls,
    options: options,
    loopAction: wall =>
    {
        // Processing logic
    }
);
```

---

## 🐛 Issue Reporting

If you encounter any problems during use or have improvement suggestions, please feel free to provide feedback through the following methods:

### GitHub Issues
[https://github.com/ViewSuSu/Su.Revit.UI.StatusBarEx/issues](https://github.com/ViewSuSu/Su.Revit.UI.StatusBarEx/issues)

### Gitee Issues
[https://gitee.com/SususuChang/status-bar-ex/issues](https://gitee.com/SususuChang/status-bar-ex/issues)

### Issue Template
To better understand and resolve issues, please include the following information when submitting an Issue:

```markdown
## Problem Description
[Clearly describe the problem encountered]

## Reproduction Steps
1. 
2. 
3. 

## Expected Behavior
[Describe expected result]

## Actual Behavior
[Describe actual result]

## Environment Information
- Revit Version: [e.g., Revit 2020]
- .NET Framework Version: [e.g., 4.8]
- Operating System: [e.g., Windows 10]
- Component Version: [e.g., 1.0.0]

## Error Log/Screenshot
[If you have error logs or screenshots, please provide here]
```

## 🤝 Contribution Guide

We welcome and appreciate all forms of contributions!

### How to Contribute
1. Fork this repository
2. Create feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Open Pull Request

### Development Environment Requirements
- Visual Studio 2022 or higher

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

Thanks to the following open-source projects:
- [ricaun.Revit.UI.StatusBar](https://github.com/ricaun-io/ricaun.Revit.UI.StatusBar) - Original project foundation
- All contributors and users for their support

---

**If this project is helpful to you, please give it a ⭐ Star!**
