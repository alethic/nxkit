Type.registerNamespace('NXKit.XForms.Web.UI');

NXKit.XForms.Web.UI.InputStringControl = function (element)
{
    NXKit.XForms.Web.UI.InputStringControl.initializeBase(this, [element]);

    this._formView = null;
    this._modelItemId = null;
    this._radTextBox = null;
};

NXKit.XForms.Web.UI.InputStringControl.prototype =
{
    initialize: function ()
    {
        NXKit.XForms.Web.UI.InputStringControl.callBaseMethod(this, 'initialize');
    },

    dispose: function ()
    {
        NXKit.XForms.Web.UI.InputStringControl.callBaseMethod(this, 'dispose');
    },

    get_formView: function ()
    {
        return this._formView;
    },
    set_formView: function (value)
    {
        if (this._formView != value)
        {
            if (this._formView != undefined)
                this._formView.remove_valueChanged(Function.createDelegate(this, this._valueChangedHandler));

            this._formView = value;
            this._formView.add_valueChanged(Function.createDelegate(this, this._valueChangedHandler));
            this.raisePropertyChanged('formView');
        }
    },

    get_modelItemId: function()
    {
        return this._modelItemId;
    },

    set_modelItemId: function(value)
    {
        if (this._modelItemId != value)
        {
            this._modelItemId = value;
            this.raisePropertyChanged('modelItemId');
        }
    },

    get_radTextBox: function()
    {
        return this._radTextBox;
    },

    set_radTextBox: function(value)
    {
        if (this._radTextBox != value)
        {
            if (this._radTextBox != null)
                this._radTextBox.remove_valueChanged(Function.createDelegate(this, this._onTextBoxValueChangedHandler));

            this._radTextBox = value;
            this.raisePropertyChanged('radTextBox');

            this._radTextBox.add_valueChanged(Function.createDelegate(this, this._onTextBoxValueChangedHandler));
        }
    },

    _valueChangedHandler: function (source, args)
    {
        // does this apply to us
        if (args.get_modelItemId() != this.get_modelItemId())
            return;

        if (args.get_newValue() != null)
            this.get_radTextBox().set_value(args.get_newValue());
    },
    
    _onTextBoxValueChangedHandler: function (source, args)
    {
        this.get_formView().raiseValueChanged(new NXKit.Web.UI.ValueChangedEventArgs(self, this.get_modelItemId(), source.get_value(), null));
    },
};

NXKit.XForms.Web.UI.InputStringControl.registerClass('NXKit.XForms.Web.UI.InputStringControl', Sys.UI.Control);

if (typeof(Sys) !== 'undefined')
    Sys.Application.notifyScriptLoaded();
