import Vue from 'vue'
import Router from 'vue-router'
import Dashboard from '@/components/Dashboard'
import Accounts from '@/components/accounts/Accounts'
import Categories from '@/components/categories/Categories'
import Payments from '@/components/payments/Payments'

Vue.use(Router);

export default new Router({
	routes: [
		{
			path: '/',
			name: 'Dashboard',
			component: Dashboard
		},
		{
			path: '/accounts',
			name: 'Accounts',
			component: Accounts
		},
		{
			path: '/categories',
			name: 'Categories',
			component: Categories
		}, {
			path: '/payments',
			name: 'Payments',
			component: Payments
		}
	]
})
