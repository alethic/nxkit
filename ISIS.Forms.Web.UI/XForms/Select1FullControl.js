Type.registerNamespace('ISIS.Forms.Web.UI.XForms');

ISIS.Forms.Web.UI.XForms.Select1FullControl = function (element)
{
    ISIS.Forms.Web.UI.XForms.Select1FullControl.initializeBase(this, [element]);

    this._formView = null;
    this._modelItemId = null;
};

ISIS.Forms.Web.UI.XForms.Select1FullControl.prototype =
{
    initialize: function ()
    {
        ISIS.Forms.Web.UI.XForms.Select1FullControl.callBaseMethod(this, 'initialize');

        var self = this;

        FormView_jQuery(this.get_element()).find("input").each(function ()
        {
            $addHandler(this, "click", function()
            {
                self._onInputClickHandler(this);
            });
        });
    },

    dispose: function ()
    {
        ISIS.Forms.Web.UI.XForms.Select1FullControl.callBaseMethod(this, 'dispose');
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

    _valueChangedHandler: function (source, args)
    {
        // has the value of our node has changed
        if (args.get_modelItemId() != this.get_modelItemId())
            return;

        FormView_jQuery(this.get_element()).find("input").each(function ()
        {
            this.checked = parseInt(this.value) == args.get_newValueHashCode();
        });
    },

    _onInputClickHandler: function (input)
    {
        this.get_formView().raiseValueChanged(new ISIS.Forms.Web.UI.ValueChangedEventArgs(self, this.get_modelItemId(), null, parseInt(input.value)));
    },
};

ISIS.Forms.Web.UI.XForms.Select1FullControl.registerClass('ISIS.Forms.Web.UI.XForms.Select1FullControl', Sys.UI.Control);

if (typeof(Sys) !== 'undefined')
    Sys.Application.notifyScriptLoaded();
