import {createRootRoute, createRoute, createRouter, Outlet} from "@tanstack/react-router";
import { BooksPage } from "@/pages/books/Books.page";
import {RootLayout} from "@/layout/rootLayout.tsx";


const rootRoute = createRootRoute({
	component: () => <RootLayout> <Outlet /> </RootLayout>,
})

export const indexRoute = createRoute({
	getParentRoute: () => rootRoute,
	path: '/',
	component: () => <p>Hello World!</p>,
})

export const booksRoute = createRoute({
	getParentRoute: () => rootRoute,
	path: '/books',
	component: () => <BooksPage/>,
})

const routeTree = rootRoute.addChildren([indexRoute, booksRoute])

export const router = createRouter({
	routeTree,
	defaultPreload: 'intent',
	scrollRestoration: true,
})
