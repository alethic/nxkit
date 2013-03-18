Type.registerNamespace('NXKit.XForms.Layout.Web.UI');

NXKit.XForms.Layout.Web.UI.TableControl = function (element)
{
    NXKit.XForms.Layout.Web.UI.TableControl.initializeBase(this, [element]);

    this._view = null;
};

NXKit.XForms.Layout.Web.UI.TableControl.prototype =
{
    initialize: function ()
    {
        NXKit.XForms.Layout.Web.UI.TableControl.callBaseMethod(this, 'initialize');
        
        $addHandler(window,   'load', Function.createDelegate(this, this._onLoadHandler));
        $addHandler(window, 'resize', Function.createDelegate(this, this._onResizeHandler));
    },

    dispose: function ()
    {
        NXKit.XForms.Layout.Web.UI.TableControl.callBaseMethod(this, 'dispose');
    },

    get_view: function ()
    {
        return this._view;
    },
    set_view: function (value)
    {
        if (this._view != value)
        {
            this._view = value;
            this.raisePropertyChanged('view');
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

NXKit.XForms.Layout.Web.UI.TableControl.registerClass('NXKit.XForms.Layout.Web.UI.TableControl', Sys.UI.Control);

if (typeof(Sys) !== 'undefined')
    Sys.Application.notifyScriptLoaded();
