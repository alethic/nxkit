Type.registerNamespace('ISIS.Forms.Web.UI.XForms');

ISIS.Forms.Web.UI.XForms.Select1MinimalControl = function (element)
{
    ISIS.Forms.Web.UI.XForms.Select1MinimalControl.initializeBase(this, [element]);

    this._formView = null;
    this._modelItemId = null;
    this._radComboBox = null;
};

ISIS.Forms.Web.UI.XForms.Select1MinimalControl.prototype =
{
    initialize: function ()
    {
        ISIS.Forms.Web.UI.XForms.Select1MinimalControl.callBaseMethod(this, 'initialize');
    },

    dispose: function ()
    {
        ISIS.Forms.Web.UI.XForms.Select1MinimalControl.callBaseMethod(this, 'dispose');
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

    get_radComboBox: function()
    {
        return this._radComboBox;
    },

    set_radComboBox: function(value)
    {
        if (this._radComboBox != value)
        {
            if (this._radComboBox != null)
                this._radComboBox.remove_selectedIndexChanged(Function.createDelegate(this, this._onComboBoxSelectedIndexChangedHandler));

            this._radComboBox = value;
            this.raisePropertyChanged('radComboBox');

            this._radComboBox.add_selectedIndexChanged(Function.createDelegate(this, this._onComboBoxSelectedIndexChangedHandler));
        }
    },

    _valueChangedHandler: function (source, args)
    {
        // does this apply to us
        if (args.get_modelItemId() != this.get_modelItemId())
            return;

        // select item
        this.get_radComboBox().set_value(args.get_newValueHashCode());
    },

    _onComboBoxSelectedIndexChangedHandler: function (source, args)
    {
        var value = 0;
        var item = args.get_item();
        if (item != null)
            value = parseInt(item.get_value());

        this.get_formView().raiseValueChanged(new ISIS.Forms.Web.UI.ValueChangedEventArgs(self, this.get_modelItemId(), null, value));
    },
};

ISIS.Forms.Web.UI.XForms.Select1MinimalControl.registerClass('ISIS.Forms.Web.UI.XForms.Select1MinimalControl', Sys.UI.Control);

if (typeof(Sys) !== 'undefined')
    Sys.Application.notifyScriptLoaded();
