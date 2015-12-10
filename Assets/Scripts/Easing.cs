public class Easing {
	public class Quadratic {
		public static float Out( float t ) {
			return -t * ( t - 2 );
		}

		public static float In( float t ) {
			return t * t;
		}

		public static float InOut( float t ) {
			if ( t < 0.5 )
				return 2 * t * t;
			else
				return -0.5f * ( ( 2*t-1 ) * ( 2*( t-1 )-2 ) - 1 );
		}
	}
}
