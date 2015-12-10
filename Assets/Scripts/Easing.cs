using UnityEngine;

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

	public class Sine {
		public static float Out( float t ) {
			return Mathf.Sin (t * (Mathf.PI / 2));
		}

		public static float In( float t ) {
			return 1 - Mathf.Sin ( t * (Mathf.PI / 2 ));
		}

		public static float InOut( float t ) {
			if (t < 0.5)
				return 0.5f * (Mathf.Sin (Mathf.PI * t));
			else
				return -0.5f * (Mathf.Cos (Mathf.PI * (2 * t - 1) / 2) - 2);
		}
	}
}
