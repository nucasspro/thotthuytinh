<?php
/**
 * The base configuration for WordPress
 *
 * The wp-config.php creation script uses this file during the
 * installation. You don't have to use the web site, you can
 * copy this file to "wp-config.php" and fill in the values.
 *
 * This file contains the following configurations:
 *
 * * MySQL settings
 * * Secret keys
 * * Database table prefix
 * * ABSPATH
 *
 * @link https://codex.wordpress.org/Editing_wp-config.php
 *
 * @package WordPress
 */

// ** MySQL settings - You can get this info from your web host ** //
/** The name of the database for WordPress */
define('DB_NAME', 'thotthuytinh');

/** MySQL database username */
define('DB_USER', 'root');

/** MySQL database password */
define('DB_PASSWORD', '');

/** MySQL hostname */
define('DB_HOST', 'localhost');

/** Database Charset to use in creating database tables. */
define('DB_CHARSET', 'utf8mb4');

/** The Database Collate type. Don't change this if in doubt. */
define('DB_COLLATE', '');

/**#@+
 * Authentication Unique Keys and Salts.
 *
 * Change these to different unique phrases!
 * You can generate these using the {@link https://api.wordpress.org/secret-key/1.1/salt/ WordPress.org secret-key service}
 * You can change these at any point in time to invalidate all existing cookies. This will force all users to have to log in again.
 *
 * @since 2.6.0
 */
define('AUTH_KEY',         'S!Q=uE~`|dK4mhsWhBLhZ{6kNoPe<A*OI<~U@Yjl82r;nua}uMiwPVgDf*+o;,bE');
define('SECURE_AUTH_KEY',  '}<{f(1)hatS&jfce02k/yWQaoN0$lJ8;iP:Q6fmV[V[HbN[0woZfRAeaJLr`-@P}');
define('LOGGED_IN_KEY',    '&9Gx!=p|d|}2;H(-BOCg7~rqgw4^R/_n.4T0C-s6S;c=4)n<Ga9Bb-B;@_KgqZ_)');
define('NONCE_KEY',        'MSO0OYxP_8Nagj8o2<!B#O9vHK#V6;3: =;|Hl.eL!<%CzbK@Vf;Jild3lDg$i;>');
define('AUTH_SALT',        '6^z)nCF Y2k,[)?}s775e-pX3d?}c`lLmL2$UplU|)0-(v*K6[c:tI&;b*7<Yuo%');
define('SECURE_AUTH_SALT', '#8D::RqF1CpGTQeZfg2W NWd/y_aJoG@KPFY$%Tc!,Tud]H-=O~.i [a2;r/3qRs');
define('LOGGED_IN_SALT',   'AHZ4z{TpNw_0-MH<eB=i>-d KmPu`Hs<17&wScB=RY?jjFs<cCl+l^*paaZ:&<HI');
define('NONCE_SALT',       'lg8j]!X-d[#ps(bbPfgw%Bm2E7j*Y E.fjt?=m=Qqf8.]@bx/5_S}hS2wSjwkR]+');

/**#@-*/

/**
 * WordPress Database Table prefix.
 *
 * You can have multiple installations in one database if you give each
 * a unique prefix. Only numbers, letters, and underscores please!
 */
$table_prefix  = 'wp_';

/**
 * For developers: WordPress debugging mode.
 *
 * Change this to true to enable the display of notices during development.
 * It is strongly recommended that plugin and theme developers use WP_DEBUG
 * in their development environments.
 *
 * For information on other constants that can be used for debugging,
 * visit the Codex.
 *
 * @link https://codex.wordpress.org/Debugging_in_WordPress
 */
define('WP_DEBUG', false);

/* That's all, stop editing! Happy blogging. */

/** Absolute path to the WordPress directory. */
if ( !defined('ABSPATH') )
	define('ABSPATH', dirname(__FILE__) . '/');

/** Sets up WordPress vars and included files. */
require_once(ABSPATH . 'wp-settings.php');
