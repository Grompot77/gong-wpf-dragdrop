using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GongComplexType.Model;
using GongSolutions.Wpf.DragDrop;

namespace GongComplexType.ViewModels
{
    public enum ActionEnum
    {
        Move,
        Copy,
        CopyDistinct,
        Remove
    }

    public class SomeViewModel : IDropTarget
    {
        private ObservableCollection<SomeObject> _mainCollection = new ObservableCollection<SomeObject>();

        public SomeViewModel()
        {
            Initialize();
        }

        public IEnumerable<ActionEnum> ActionEnums
        {
            get { return Enum.GetValues(typeof(ActionEnum)).Cast<ActionEnum>(); }
        }

        public ActionEnum Action { get; set; }

        Random _rnd = new Random((int)DateTime.Now.Ticks);
        public void Initialize()
        {
            for (int i = 0; i <= 20; ++i)
            {
                _mainCollection.Add(new SomeObject
                {
                    FirstName = string.Format("First Name {0}", i),
                    Surname = string.Format("Surname {0}", i),
                    DoB = DateTime.Today.AddDays(-_rnd.Next(365 * 10, 365 * 70))
                });
            }
        }

        private ObservableCollection<SomeObject> _selected = new ObservableCollection<SomeObject>();
        public ObservableCollection<SomeObject> Selected
        {
            get { return _selected; }
        }

        public ObservableCollection<SomeObject> MainCollection
        {
            get
            {
                return _mainCollection;
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            var target = dropInfo.TargetCollection as IEnumerable<SomeObject>;
            if (target != null)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                switch (Action)
                {
                    case ActionEnum.Copy:
                        dropInfo.Effects = DragDropEffects.Copy; //always copy
                        break;
                    case ActionEnum.CopyDistinct:
                        dropInfo.Effects = DragDropEffects.Copy; //copy, but exclude any duplicates
                        break;
                    case ActionEnum.Move:
                        dropInfo.Effects = DragDropEffects.Move; //copy and remove from source collection
                        break;
                    case ActionEnum.Remove:
                        // sorry, there was no delete drop effect and none stops it from working, but think the adorner is appropriate
                        dropInfo.Effects = DragDropEffects.Scroll;  //remove items from source collection
                        break;
                }
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            var data = dropInfo.GetData<SomeObject>(dropInfo.Data);

            // did have a look if i could pick up the Effect to determine if it is copy or move
            // would be nice to have the following extensions added to the framework for reuse

            switch (Action)
            {
                case ActionEnum.Copy:
                    dropInfo.CopyDataToTarget(data); //always copy
                    break;
                case ActionEnum.CopyDistinct:
                    dropInfo.CopyDataToTarget(data, true); //copy, but exclude any duplicates
                    break;
                case ActionEnum.Move:
                    dropInfo.MoveDataToTarget(data); //copy and remove from source collection
                    break;
                case ActionEnum.Remove:
                    dropInfo.RemoveFromSource(data);  //remove items from source collection
                    break;
            }
        }
    }

    public static class Extensions
    {
        public static IEnumerable<T> GetData<T>(this IDropInfo info, object data)
        {
            if (typeof(T).IsInstanceOfType(data))
                return new[] { (T)data };
            if (typeof(IEnumerable<T>).IsInstanceOfType(data))
                return (IEnumerable<T>)data;
            return Enumerable.Empty<T>();
        }

        public static void RemoveFromSource<T>(this IDropInfo info, IEnumerable<T> data)
        {
            foreach (var d in data)
                ((IList)info.DragInfo.SourceCollection).Remove(d);
        }

        public static void MoveDataToTarget<T>(this IDropInfo info, IEnumerable<T> data)
        {
            info.CopyDataToTarget(data);
            info.RemoveFromSource(data);
        }

        public static void CopyDataToTarget<T>(this IDropInfo info, IEnumerable<T> data)
        {
            info.CopyDataToTarget(data, false);
        }

        public static void CopyDataToTarget<T>(this IDropInfo info, IEnumerable<T> data, bool distinct)
        {
            foreach (var d in data)
            {
                if (((IList)info.TargetCollection).Contains(d) && distinct)
                    continue;
                ((IList)info.TargetCollection).Add(d);
            }
        }
    }
}
