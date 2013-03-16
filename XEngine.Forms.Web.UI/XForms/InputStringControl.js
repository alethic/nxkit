Type.registerNamespace('ISIS.Forms.Web.UI.XForms');

ISIS.Forms.Web.UI.XForms.InputStringControl = function (element)
{
    ISIS.Forms.Web.UI.XForms.InputStringControl.initializeBase(this, [element]);

    this._formView = null;
    this._modelItemId = null;
    this._radTextBox = null;
};

ISIS.Forms.Web.UI.XForms.InputStringControl.prototype =
{
    initialize: function ()
    {
        ISIS.Forms.Web.UI.XForms.InputStringControl.callBaseMethod(this, 'initialize');
    },

    dispose: function ()
    {
        ISIS.Forms.Web.UI.XForms.InputStringControl.callBaseMethod(this, 'dispose');
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
        this.get_formView().raiseValueChanged(new ISIS.Forms.Web.UI.ValueChangedEventArgs(self, this.get_modelItemId(), source.get_value(), null));
    },
};

ISIS.Forms.Web.UI.XForms.InputStringControl.registerClass('ISIS.Forms.Web.UI.XForms.InputStringControl', Sys.UI.Control);

if (typeof(Sys) !== 'undefined')
    Sys.Application.notifyScriptLoaded();
