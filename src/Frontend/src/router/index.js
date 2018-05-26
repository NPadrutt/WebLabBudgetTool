import Vue from 'vue'
import Router from 'vue-router'
import AuthService from '@/service/AuthService'
import Dashboard from '@/components/Dashboard'
import Accounts from '@/components/accounts/Accounts'
import Categories from '@/components/categories/Categories'
import Payments from '@/components/payments/Payments'
import Login from '@/components/Login'

Vue.use(Router);

export default new Router({
	routes: [
		{
			path: '/login/:redirectTarget?',
			name: 'Login',
			component: Login,
			props: true,
		},
		{
			path: '/',
			name: 'Dashboard',
			component: Dashboard,
			beforeEnter: requireAuthentication,
			children: [
				{
					path: 'accounts',
					name: 'Accounts',
					component: Accounts,
					beforeEnter: requireAuthentication
				},
				{
					path: 'categories',
					name: 'Categories',
					component: Categories,
					// beforeEnter: requireAuthentication
				}, {
					path: 'payments',
					name: 'Payments',
					component: Payments,
					beforeEnter: requireAuthentication
				}
			]
		},
	]
})

function requireAuthentication(to, from, next) {
	if (!AuthService.loggedIn()) {
		next({
			name: 'Login',
			params: {
				redirectTarget: to.fullPath
			}
		})
	} else {
		next()
	}
}