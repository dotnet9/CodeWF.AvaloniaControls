﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace CodeWF.AvaloniaControls.Extensions;

public class RangeObservableCollection<T> : ObservableCollection<T>
{
    private bool SuppressNotification { get; set; }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        if (!SuppressNotification) base.OnCollectionChanged(e);
    }

    public new void Clear()
    {
        Items.Clear();
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public void Add(IEnumerable<T> collection)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        SuppressNotification = true;

        foreach (var item in collection) Items.Add(item);

        SuppressNotification = false;
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public void Insert(int index, IEnumerable<T> collection)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        SuppressNotification = true;

        foreach (var item in collection) Items.Insert(index, item);

        SuppressNotification = false;
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
}