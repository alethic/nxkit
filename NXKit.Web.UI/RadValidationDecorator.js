Type.registerNamespace("NXKit.Web.UI.RadValidationDecorator");

NXKit.Web.UI.RadValidationDecorator.RadInputValidationDecorator_ValidationFunction = function (source, arguments)
{
    if (source.controltovalidate == undefined)
        return;

    // obtain RadInput control
    var txt = $find(source.controltovalidate);

    // validate address
    arguments.IsValid = true;

    for (i in txt._element.Validators)
    {
        // skip self
        var validator = txt._element.Validators[i];
        if (validator == source)
            continue;

        if (!validator.isvalid)
        {
            // not valid, update input and exit
            NXKit.Web.UI.RadValidationDecorator.RadInputValidationDecorator_SetInvalid(txt, true);
            return;
        }
    }

    // if we made it this far, all validators are valid
    NXKit.Web.UI.RadValidationDecorator.RadInputValidationDecorator_SetInvalid(txt, false);
}

NXKit.Web.UI.RadValidationDecorator.RadInputValidationDecorator_SetInvalid = function (ctl, invalid)
{
    ctl._invalid = invalid;
    ctl.updateCssClass();
}

NXKit.Web.UI.RadValidationDecorator.RadInputValidationDecorator_LoadInvalidate = function (ctl)
{
    // set invalid state
    NXKit.Web.UI.RadValidationDecorator.RadInputValidationDecorator_SetInvalid(ctl, true);
}

NXKit.Web.UI.RadValidationDecorator.RadComboBoxValidationDecorator_ValidationFunction = function (source, arguments)
{
    if (source.controltovalidate == undefined)
        return;

    // obtain RadInput control
    var txt = $find(source.controltovalidate);

    // validate address
    arguments.IsValid = true;

    for (i in txt._element.Validators)
    {
        // skip self
        var validator = txt._element.Validators[i];
        if (validator == source)
            continue;

        if (!validator.isvalid)
        {
            // not valid, update input and exit
            txt._invalid = true;
            txt.updateCssClass();
            return;
        }
    }

    // if we made it this far, all validators are valid
    txt._invalid = false;
    txt.updateCssClass();
}
