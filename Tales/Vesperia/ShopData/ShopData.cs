﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HyoutaTools.Tales.Vesperia.ShopData {
	public class ShopData {
		public ShopData( String filename, uint shopStart, uint shopCount, uint itemStart, uint itemCount, Util.Endianness endian, Util.Bitness bits ) {
			using ( Stream stream = new System.IO.FileStream( filename, FileMode.Open, System.IO.FileAccess.Read ) ) {
				if ( !LoadFile( stream, shopStart, shopCount, itemStart, itemCount, endian, bits ) ) {
					throw new Exception( "Loading ShopData failed!" );
				}
			}
		}

		public ShopData( Stream stream, uint shopStart, uint shopCount, uint itemStart, uint itemCount, Util.Endianness endian, Util.Bitness bits ) {
			if ( !LoadFile( stream, shopStart, shopCount, itemStart, itemCount, endian, bits ) ) {
				throw new Exception( "Loading ShopData failed!" );
			}
		}

		public List<ShopDefinition> ShopDefinitions;
		public List<ShopItem> ShopItems;
		public Dictionary<uint, ShopDefinition> ShopDictionary;

		private bool LoadFile( Stream stream, uint shopStart, uint shopCount, uint itemStart, uint itemCount, Util.Endianness endian, Util.Bitness bits ) {
			ShopDefinitions = new List<ShopDefinition>( (int)shopCount );
			ShopItems = new List<ShopItem>( (int)itemCount );

			for ( int i = 0; i < shopCount; ++i ) {
				stream.Position = shopStart + i * ( 28 + bits.NumberOfBytes() );
				var shop = new ShopDefinition( stream, endian, bits );
				ShopDefinitions.Add( shop );
			}

			for ( int i = 0; i < itemCount; ++i ) {
				stream.Position = itemStart + i * 56;
				var item = new ShopItem( stream, endian );
				ShopItems.Add( item );
			}

			foreach ( var shop in ShopDefinitions ) {
				shop.ShopItems = ShopItems.Where( x => x.ShopID == shop.InGameID ).ToArray();
			}

			ShopDictionary = new Dictionary<uint, ShopDefinition>();
			foreach ( var shop in ShopDefinitions ) {
				ShopDictionary.Add( shop.InGameID, shop );
			}

			return true;
		}
	}
}
