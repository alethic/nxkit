Type.registerNamespace('ISIS.Forms.Web.UI.Layout');

ISIS.Forms.Web.UI.Layout.TableControl = function (element)
{
    ISIS.Forms.Web.UI.Layout.TableControl.initializeBase(this, [element]);

    this._formView = null;
};

ISIS.Forms.Web.UI.Layout.TableControl.prototype =
{
    initialize: function ()
    {
        ISIS.Forms.Web.UI.Layout.TableControl.callBaseMethod(this, 'initialize');
        
        $addHandler(window,   'load', Function.createDelegate(this, this._onLoadHandler));
        $addHandler(window, 'resize', Function.createDelegate(this, this._onResizeHandler));
    },

    dispose: function ()
    {
        ISIS.Forms.Web.UI.Layout.TableControl.callBaseMethod(this, 'dispose');
    },

    get_formView: function ()
    {
        return this._formView;
    },
    set_formView: function (value)
    {
        if (this._formView != value)
        {
            this._formView = value;
            this.raisePropertyChanged('formView');
        }
    },

    _onResizeHandler: function (e)
    {
//        this._resizeElement();
    },

    _onLoadHandler: function (e)
    {
//        this._resizeElement();
    },

    _resizeElement: function ()
    {
        // best width for cells
        var cellWidths = new Array();

        // all representations of this column group
        var columnGroups = FormView_jQuery(this.get_element()).children('.Layout_Table_ColumnGroup__0');

        // for each group
        columnGroups.each(function ()
        {
            // for each cell, with index
            FormView_jQuery(this).find('.Layout_Table_Cell').each(function (i)
            {
                // store maximum width
                if (!cellWidths[i] || cellWidths[i] < this.clientWidth)
                    cellWidths[i] = this.clientWidth;
            });
        });

        // for each group
        columnGroups.each(function ()
        {
            // for each cell with index
            FormView_jQuery(this).find('.Layout_Table_Cell').each(function (i)
            {
                // set width to maximum
                this.style.minwidth = cellWidths[i] + 'px';
            });
        });
    },
};

ISIS.Forms.Web.UI.Layout.TableControl.registerClass('ISIS.Forms.Web.UI.Layout.TableControl', Sys.UI.Control);

if (typeof(Sys) !== 'undefined')
    Sys.Application.notifyScriptLoaded();
