-- RedEngine in-game UI prototype for YOUR Roblox experience.
-- Install as a LocalScript under StarterPlayer > StarterPlayerScripts in Roblox Studio.
-- This is a native ScreenGui; it does not inject into or modify the Roblox client.
-- Home toggles the menu. Script text is an editor UI only; it does not dynamically execute code.

local Players = game:GetService("Players")
local UserInputService = game:GetService("UserInputService")

local player = Players.LocalPlayer
local playerGui = player:WaitForChild("PlayerGui")

local old = playerGui:FindFirstChild("RedEngineGui")
if old then old:Destroy() end

local RED = Color3.fromRGB(230, 0, 40)
local BLACK = Color3.fromRGB(9, 9, 11)
local TILE = Color3.fromRGB(21, 23, 27)
local WHITE = Color3.fromRGB(245, 245, 247)
local MUTED = Color3.fromRGB(175, 180, 190)

local gui = Instance.new("ScreenGui")
gui.Name = "RedEngineGui"
gui.ResetOnSpawn = false
gui.IgnoreGuiInset = true
gui.DisplayOrder = 100
gui.Parent = playerGui

local root = Instance.new("Frame")
root.Name = "Window"
root.AnchorPoint = Vector2.new(0.5, 0.5)
root.Position = UDim2.fromScale(0.5, 0.5)
root.Size = UDim2.fromOffset(620, 430)
root.BackgroundColor3 = BLACK
root.BorderSizePixel = 0
root.Visible = true
root.Parent = gui
Instance.new("UICorner", root).CornerRadius = UDim.new(0, 8)

local stroke = Instance.new("UIStroke")
stroke.Color = RED
stroke.Thickness = 1.5
stroke.Parent = root

local header = Instance.new("Frame")
header.Size = UDim2.new(1, 0, 0, 48)
header.BackgroundColor3 = Color3.fromRGB(14, 14, 17)
header.BorderSizePixel = 0
header.Parent = root

local brand = Instance.new("TextLabel")
brand.BackgroundTransparency = 1
brand.Position = UDim2.fromOffset(16, 0)
brand.Size = UDim2.new(1, -150, 1, 0)
brand.Font = Enum.Font.GothamBold
brand.Text = "red"
brand.TextColor3 = RED
brand.TextSize = 22
brand.TextXAlignment = Enum.TextXAlignment.Left
brand.Parent = header

local brand2 = Instance.new("TextLabel")
brand2.BackgroundTransparency = 1
brand2.Position = UDim2.fromOffset(53, 0)
brand2.Size = UDim2.fromOffset(130, 48)
brand2.Font = Enum.Font.GothamBold
brand2.Text = "ENGINE"
brand2.TextColor3 = WHITE
brand2.TextSize = 22
brand2.TextXAlignment = Enum.TextXAlignment.Left
brand2.Parent = header

local hotkey = Instance.new("TextLabel")
hotkey.BackgroundTransparency = 1
hotkey.AnchorPoint = Vector2.new(1, 0)
hotkey.Position = UDim2.new(1, -14, 0, 0)
hotkey.Size = UDim2.fromOffset(130, 48)
hotkey.Font = Enum.Font.Gotham
hotkey.Text = "HOME  •  TOGGLE"
hotkey.TextColor3 = MUTED
hotkey.TextSize = 11
hotkey.Parent = header

local nav = Instance.new("Frame")
nav.Position = UDim2.fromOffset(14, 62)
nav.Size = UDim2.fromOffset(150, 354)
nav.BackgroundTransparency = 1
nav.Parent = root

local navLayout = Instance.new("UIListLayout")
navLayout.Padding = UDim.new(0, 7)
navLayout.SortOrder = Enum.SortOrder.LayoutOrder
navLayout.Parent = nav

local content = Instance.new("Frame")
content.Position = UDim2.fromOffset(178, 62)
content.Size = UDim2.new(1, -192, 1, -78)
content.BackgroundColor3 = Color3.fromRGB(13, 14, 17)
content.BorderSizePixel = 0
content.Parent = root
Instance.new("UICorner", content).CornerRadius = UDim.new(0, 5)

local pageTitle = Instance.new("TextLabel")
pageTitle.BackgroundTransparency = 1
pageTitle.Position = UDim2.fromOffset(16, 10)
pageTitle.Size = UDim2.new(1, -32, 0, 32)
pageTitle.Font = Enum.Font.GothamBold
pageTitle.Text = "Home"
pageTitle.TextColor3 = WHITE
pageTitle.TextSize = 20
pageTitle.TextXAlignment = Enum.TextXAlignment.Left
pageTitle.Parent = content

local body = Instance.new("Frame")
body.BackgroundTransparency = 1
body.Position = UDim2.fromOffset(16, 52)
body.Size = UDim2.new(1, -32, 1, -64)
body.Parent = content

local function clearBody()
    for _, child in ipairs(body:GetChildren()) do
        child:Destroy()
    end
end

local function label(parent, text, pos, size, textSize, color)
    local item = Instance.new("TextLabel")
    item.BackgroundTransparency = 1
    item.Position = pos
    item.Size = size
    item.Font = Enum.Font.Gotham
    item.Text = text
    item.TextColor3 = color or WHITE
    item.TextSize = textSize or 14
    item.TextWrapped = true
    item.TextXAlignment = Enum.TextXAlignment.Left
    item.TextYAlignment = Enum.TextYAlignment.Top
    item.Parent = parent
    return item
end

local function showPage(name)
    pageTitle.Text = name
    clearBody()

    if name == "Executor" then
        label(body, "Luau workspace • editor UI", UDim2.fromOffset(0, 0), UDim2.new(1, 0, 0, 22), 13, MUTED)
        local editor = Instance.new("TextBox")
        editor.Name = "ScriptEditor"
        editor.Position = UDim2.fromOffset(0, 28)
        editor.Size = UDim2.new(1, 0, 1, -76)
        editor.BackgroundColor3 = Color3.fromRGB(7, 8, 10)
        editor.BorderColor3 = Color3.fromRGB(55, 57, 64)
        editor.ClearTextOnFocus = false
        editor.MultiLine = true
        editor.TextXAlignment = Enum.TextXAlignment.Left
        editor.TextYAlignment = Enum.TextYAlignment.Top
        editor.Font = Enum.Font.Code
        editor.TextSize = 14
        editor.TextColor3 = WHITE
        editor.Text = "-- RedEngine Luau editor\n-- Add gameplay scripts to your own experience in Roblox Studio.\n\nprint(\"Hello from RedEngine\")"
        editor.Parent = body
        label(body, "Editor preview only. This UI does not execute arbitrary code.", UDim2.new(0, 0, 1, -40), UDim2.new(1, 0, 0, 36), 12, MUTED)
    elseif name == "Home" then
        label(body, "REDENGINE", UDim2.fromOffset(0, 0), UDim2.new(1, 0, 0, 36), 24, RED)
        label(body, "In-game menu loaded. Press Home to hide or show this interface.", UDim2.fromOffset(0, 44), UDim2.new(1, 0, 0, 48), 14, WHITE)
        label(body, "Use Roblox Studio scripts and APIs to implement gameplay features for your own experience.", UDim2.fromOffset(0, 104), UDim2.new(1, 0, 0, 70), 13, MUTED)
    else
        local descriptions = {
            Dumper = "Local experience diagnostics and inspectable game data you own.",
            Resources = "Project resource management can be connected to your game's own data.",
            Menus = "Place for authorized in-experience gameplay tools.",
            Settings = "UI preferences and accessibility options.",
            Misc = "Utilities for your own Roblox experience.",
            Chat = "Connect this page to your experience's supported chat or community features."
        }
        label(body, descriptions[name] or "RedEngine module", UDim2.fromOffset(0, 0), UDim2.new(1, 0, 0, 80), 14, WHITE)
        label(body, "Module UI is ready for experience-specific implementation.", UDim2.fromOffset(0, 92), UDim2.new(1, 0, 0, 60), 12, MUTED)
    end
end

local pages = {
    {"〈/〉", "Executor"},
    {"▰", "Dumper"},
    {"☷", "Resources"},
    {"▤", "Menus"},
    {"⚙", "Settings"},
    {"⬡", "Misc"},
    {"●●", "Chat"},
}

for index, data in ipairs(pages) do
    local icon, name = data[1], data[2]
    local button = Instance.new("TextButton")
    button.Name = name .. "Button"
    button.LayoutOrder = index
    button.Size = UDim2.new(1, 0, 0, 40)
    button.BackgroundColor3 = TILE
    button.BorderSizePixel = 0
    button.AutoButtonColor = true
    button.Font = Enum.Font.Gotham
    button.Text = icon .. "   " .. name
    button.TextColor3 = WHITE
    button.TextSize = 13
    button.Parent = nav
    Instance.new("UICorner", button).CornerRadius = UDim.new(0, 4)
    local bar = Instance.new("Frame")
    bar.AnchorPoint = Vector2.new(0, 1)
    bar.Position = UDim2.new(0, 0, 1, 0)
    bar.Size = UDim2.new(1, 0, 0, 3)
    bar.BackgroundColor3 = RED
    bar.BorderSizePixel = 0
    bar.Parent = button
    button.Activated:Connect(function()
        showPage(name)
    end)
end

showPage("Home")

UserInputService.InputBegan:Connect(function(input, processed)
    if processed then return end
    if input.KeyCode == Enum.KeyCode.Home then
        root.Visible = not root.Visible
    end
end)

-- Small-screen support
local function resizeForViewport()
    local camera = workspace.CurrentCamera
    if not camera then return end
    local viewport = camera.ViewportSize
    if viewport.X < 700 or viewport.Y < 500 then
        root.Size = UDim2.new(1, -24, 1, -24)
    else
        root.Size = UDim2.fromOffset(620, 430)
    end
end
resizeForViewport()
if workspace.CurrentCamera then
    workspace.CurrentCamera:GetPropertyChangedSignal("ViewportSize"):Connect(resizeForViewport)
end
