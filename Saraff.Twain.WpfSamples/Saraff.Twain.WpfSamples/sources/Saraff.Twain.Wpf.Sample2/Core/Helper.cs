/* Этот файл является частью примеров использования библиотеки Saraff.Twain.NET
 * © Буякевич Александр, 2016.
 * © SARAFF SOFTWARE (Кирножицкий Андрей), 2011.
 * Saraff.Twain.NET - свободная программа: вы можете перераспространять ее и/или
 * изменять ее на условиях Меньшей Стандартной общественной лицензии GNU в том виде,
 * в каком она была опубликована Фондом свободного программного обеспечения;
 * либо версии 3 лицензии, либо (по вашему выбору) любой более поздней
 * версии.
 * Saraff.Twain.NET распространяется в надежде, что она будет полезной,
 * но БЕЗО ВСЯКИХ ГАРАНТИЙ; даже без неявной гарантии ТОВАРНОГО ВИДА
 * или ПРИГОДНОСТИ ДЛЯ ОПРЕДЕЛЕННЫХ ЦЕЛЕЙ. Подробнее см. в Меньшей Стандартной
 * общественной лицензии GNU.
 * Вы должны были получить копию Меньшей Стандартной общественной лицензии GNU
 * вместе с этой программой. Если это не так, см.
 * <http://www.gnu.org/licenses/>.)
 * 
 * This file is part of samples of Saraff.Twain.NET.
 * © Buyakevich Alexander, 2016.
 * © SARAFF SOFTWARE (Kirnazhytski Andrei), 2011.
 * Saraff.Twain.NET is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * Saraff.Twain.NET is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 * You should have received a copy of the GNU Lesser General Public License
 * along with Saraff.Twain.NET. If not, see <http://www.gnu.org/licenses/>.
 * 
 * PLEASE SEND EMAIL TO:  twain@saraff.ru.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Collections.ObjectModel;

namespace Saraff.Twain.Wpf.Sample2.Core {

    internal static class Helper {

        public static void Set(this CollectionViewSource viewSource, Twain32.Enumeration cap) {
            var _values=new Collection<object>();
            for(var i=0; i<cap.Count; i++) {
                _values.Add(cap[i]);
            }
            viewSource.Source=_values;
            viewSource.View.MoveCurrentTo(cap[cap.CurrentIndex]);
        }

        public static void ErrorMessageBox(this Exception ex) {
            var _msg=string.Empty;
            for(var _ex=ex; _ex!=null; _ex=_ex.InnerException) {
                _msg=string.Format("{1}: {0}{2}{0}{0}", Environment.NewLine, _ex.Message, _ex.StackTrace);
            }
            MessageBox.Show(_msg, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
