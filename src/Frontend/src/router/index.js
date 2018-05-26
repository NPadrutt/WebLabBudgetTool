import Vue from 'vue'
import Router from 'vue-router'
import AuthService from '@/service/AuthService'
import Dashboard from '@/components/Dashboard'
import Accounts from '@/components/accounts/Accounts'
import Categories from '@/components/categories/Categories'
import Payments from '@/components/payments/Payments'
import Login from '@/components/Login'

Vue.use(Router);

const router = new Router({
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
			meta: {requiresAuthentication: true},
			children: [
				{
					path: 'accounts',
					name: 'Accounts',
					component: Accounts,
					meta: {requiresAuthentication: true},
				},
				{
					path: 'categories',
					name: 'Categories',
					component: Categories,
					meta: {requiresAuthentication: true},
				}, {
					path: 'payments',
					name: 'Payments',
					component: Payments,
					meta: {requiresAuthentication: true},
				}
			]
		},
	]
});

router.beforeEach((to, from, next) => {
	if (to.matched.some(record => record.meta.requiresAuthentication)
		&& !AuthService.loggedIn()
	) {
		next({
			name: 'Login',
			params: {
				redirectTarget: to.fullPath
			}
		})
	} else {
		next()
	}
});

export default router;