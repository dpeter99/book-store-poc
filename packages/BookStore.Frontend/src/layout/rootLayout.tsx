// @flow 
import * as React from 'react';
import {Link} from "@tanstack/react-router";
import {booksRoute, indexRoute} from "@/routes.tsx";

type Props = {
	children?: React.ReactNode;
};
export const RootLayout = (props: Props) => {
	return (
		<div>
			<NavBar />
			{props.children}
		</div>
	);
};

const NavBar = () => {
	return (
		<ul role="menubar">
			<li role="menuitem" tabIndex={0}><Link to={indexRoute.to}>Home</Link></li> 
			<li role="menuitem" tabIndex={0}><Link to={booksRoute.to}>Books</Link></li>
			<li role="menuitem" tabIndex={0}>Edit</li>
			<li role="menuitem" tabIndex={0}>View</li>
			<li role="menuitem" tabIndex={0}>Help</li>
		</ul>
	)
}
