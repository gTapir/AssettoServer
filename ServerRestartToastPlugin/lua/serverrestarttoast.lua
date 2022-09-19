local timeUntilRestartInMin = 5

local serverRestartEvent = ac.OnlineEvent({
    timeUntilRestartInMin = ac.StructItem.int32()
  }, function (sender, data)
    -- only accept packets from server
    if sender ~= nil then
      return
    end

    ac.debug("sender", sender)

    timeUntilRestartInMin = data.timeUntilRestartInMin

    ac.debug("timeUntilRestartInMin", timeUntilRestartInMin)

    ui.toast(ui.Icons.Warning, 'Server restart to update cars in X minutes')
  end)
