# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build Commands

```powershell
# Restore NuGet packages
nuget restore CheckSystem.sln

# Build solution (Debug)
msbuild CheckSystem.sln /p:Configuration=Debug /p:Platform="Any CPU"

# Build solution (Release)
msbuild CheckSystem.sln /p:Configuration=Release /p:Platform="Any CPU"

# Clean build artifacts
msbuild CheckSystem.sln /t:Clean
```

## Architecture Overview

This is an industrial equipment inspection and control system (设备检测系统) for automotive components, built with Windows Forms on .NET Framework 4.5.1.

### Solution Structure

```
CheckSystem.sln
├── CheckSystem/          # Main WinForms application (entry point)
├── Controller/           # Device controllers (CAN/LIN/PLC communication)
├── CommonUtility/        # Utilities (file ops, bus communication, cameras)
├── StateMachine/         # State machine definitions and DeviceConfig
├── DeviceDesign/         # Visual state machine designer and device configuration UI
├── UserControls/         # Custom UI controls (UserDataGrid)
├── BLL/                  # Business logic layer
├── DAL/                  # Data access layer
├── Model/                # Data entity models
└── DBUtility/            # Database utilities
```

### Key Dependencies

- **UI**: SunnyUI 3.6.5, HZH_Controls, WeifenLuo.WinFormsUI.Docking
- **Communication**: HslCommunication, SimpleTCP, TouchSocket, SuperSocket
- **Image Processing**: OpenCvSharp 4.0, EmguCV, NationalInstruments.Vision
- **Database**: SqlSugar 5.0, System.Data.SQLite
- **State Machine**: Stateless library
- **Hardware SDKs**: ZLG CAN, Toomoss USB, 海康相机, 大恒相机, NI视觉

### DeviceConfig Structure (StateMachine/DeviceConfig.cs)

The system uses a central `DeviceConfig` XML structure with these key collections:

| Collection | Purpose |
|-----------|---------|
| `Controllers` | Device controller definitions (Name, Type) |
| `Parts` | Controller field to UI name mapping (ControllerField → Name) |
| `Paras` | Process parameters linked to controller fields |
| `Processes` | Manufacturing process definitions |
| `Gears` | Gear/match settings |
| `StatusUnits` | State machine units for each workstation |
| `Conditions` | Transition conditions between states |
| `Controls` | UI control definitions |
| `WorkStations` | Workstation configurations |

### Central Communication Hub (DeviceDesign/ClassComm.cs)

`ClassComm` is the central hub for device configuration management:

- **Static properties**: `DeviceConfig`, `FilePathDeviceConfig`
- **Event system**: `ControllerNameChanged` event notifies subscribers when a controller is renamed
  - Updates controller tree (FormTree)
  - Updates open controller editor windows (FormDesignController)
  - Updates Parts mapping (UpdatePartsControllerName)
  - Updates Paras mapping (UpdateParasControllerName)
- **Helper methods**: `SaveDeviceConfigToFile()`, `GetDeviceConfigFromFile()`, `AnalysisDisplayName()`

### FormTree Node Structure

The device tree (FormTree) uses these Tag prefixes:
- `"Controller.{Type}"` - Controller nodes (e.g., `"Controller.Vw416RearLamp"`)
- `"DeviceConfig{TableName}"` - Configuration table nodes (e.g., `"DeviceConfigPart"`, `"DeviceConfigPara"`)

### Module Responsibilities

| Module | Purpose |
|--------|---------|
| `CheckSystem/` | Main UI - forms for inspection (FormCheck, FormSelection), CAN/LIN/CCD submodules |
| `Controller/` | Hardware abstraction - product-specific controllers (Vw416RearLamp, etc.), PLC, power supplies, barcode scanners |
| `CommonUtility/` | Low-level I/O - CAN/LIN bus (zlgcan.cs), CSV/Excel/XML helpers, camera SDK wrappers |
| `DeviceDesign/` | State machine visual designer (FormStatusMachineDesigner), device configuration UI forms |
| `StateMachine/` | State machine core types (StateMachineRunner, ConditionalCodeLine, ExecutableCodeLine), DeviceConfig schema |
| `UserControls/` | Custom controls including `UserDataGrid` (DataGridView with built-in search/sort support) |
| `BLL/DAL/Model/` | Traditional three-tier data layer for MES/manufacturing data |

### Important Patterns

- **Device configuration**: Stored in XML files via `ClassComm` helper class, loaded via `Deserialize<T>()`
- **State machine**: Uses `Stateless` library; state units and conditions designed via `DeviceDesign/FormStatusMachineDesigner`
- **Controller field naming**: Format is `"{ControllerName}.Field.{FieldName}"` (e.g., `"Vw416RearLamp.Field.nPowerVoltage"`)
- **Parts mapping**: Links controller fields to human-readable names for UI display
- **Communication**: CAN FD via ZLG devices, LIN via custom implementations, Modbus via HslCommunication
- **Data storage**: SQL Server (primary), SQLite (local cache)

## Code Conventions

- **Controllers**: Named by product/model (e.g., `Vw416RearLamp`, `SiemensPlc`)
- **Forms**: Prefix `Form` (e.g., `FormCheck`, `FormSelection`)
- **Data models**: Lowercase prefix + entity name (e.g., `deviceInfo`, `productCheckData`)
- **Controller fields**: Public fields with `[Description]` attributes for UI display names
- **DataGrid columns**: Access by index rather than Name property (SunnyUI UIDataGridView quirk)
- **Event subscriptions**: Always unsubscribe in `OnFormClosed` to prevent memory leaks

## UI Components

### UserDataGrid (UserControls)
- Wraps SunnyUI `UIDataGridView` with enhanced styling
- Built-in search highlighting via `PerformSearch()`
- Sort support via `ColumnHeaderMouseClick` event
- Use column index (not Name) for sorting: `row.Cells[columnIndex]`

### FormBase (DeviceDesign)
- Base class for all DeviceDesign forms
- Provides common initialization and disposal patterns