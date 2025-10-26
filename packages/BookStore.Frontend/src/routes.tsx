import {createRootRoute, createRoute, createRouter} from "@tanstack/react-router";
import { BooksPage } from "@/pages/books/Books.page";


const rootRoute = createRootRoute()

const indexRoute = createRoute({
	getParentRoute: () => rootRoute,
	path: '/',
	component: () => <p>asd</p>,
})

const booksRoute = createRoute({
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
