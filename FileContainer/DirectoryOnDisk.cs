﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyoutaTools.FileContainer {
	public class DirectoryOnDisk : ContainerBase {
		private System.IO.FileSystemInfo[] Children;

		public DirectoryOnDisk( string path ) {
			Children = new System.IO.DirectoryInfo( path ).GetFileSystemInfos();
		}

		private static INode CreateNode( System.IO.FileSystemInfo f ) {
			if ( f is System.IO.DirectoryInfo ) {
				return new DirectoryOnDisk( f.FullName );
			} else {
				return new FileOnDisk( f.FullName );
			}
		}

		public override INode GetChildByIndex( long index ) {
			if ( index > 0 && index < Children.Length ) {
				return CreateNode( Children[index] );
			}
			return null;
		}

		public override INode GetChildByName( string name ) {
			foreach ( var ch in Children ) {
				if ( ch.Name == name ) {
					return CreateNode( ch );
				}
			}
			return null;
		}

		public override void Dispose() {
		}
	}
}
