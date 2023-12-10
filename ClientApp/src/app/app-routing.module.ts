import {NgModule} from '@angular/core';
import {RouterModule, Routes} from "@angular/router";
import {SignInComponent} from "./auth/sign-in/sign-in.component";
import {SignUpComponent} from "./auth/sign-up/sign-up.component";
import {LandingComponent} from "./landing/landing.component";
import {UserComponent} from "./user/user.component";
import {ChartComponent} from "./chart/chart.component";
import {AuthGuard} from "./guards/auth-guard.service";
import {JwtModule} from "@auth0/angular-jwt";
import {tokenGetter} from "./app.module";
import {StoreComponent} from "./store/store.component";
import {EmployeeComponent} from "./employee/employee.component";
import {AddStoreComponent} from "./store/add-store/add-store.component";
import {NoAuthGuard} from "./guards/no-auth-guard.service";
import {AddEmployeeComponent} from "./employee/add-employee/add-employee.component";
import {RoleComponent} from "./employee/role/role.component";
import {CashRegisterComponent} from "./cashregister/cashregister.component";
import {AddRoleComponent} from "./employee/role/add-role/add-role.component";
import {StatusComponent} from "./cashregister/status/status.component";
import {AddStatusComponent} from "./cashregister/status/add-status/add-status.component";
import {AddCashregisterComponent} from "./cashregister/add-cashregister/add-cashregister.component";
import {LogsComponent} from "./logs/logs.component";
import {ProductComponent} from "./product/product.component";
import {PerishableComponent} from "./product/perishable/perishable.component";
import {NonperishableComponent} from "./product/nonperishable/nonperishable.component";
import {SettingsComponent} from "./settings/settings.component";
import {AddProductComponent} from "./product/add-product/add-product.component";
import {OrderComponent} from "./product/order/order/order.component";
import {AddOrderComponent} from "./product/order/add-order/add-order.component";
import {InventoryComponent} from "./inventory/inventory.component";
import {WarehouseComponent} from "./warehouse/warehouse.component";
import {SaleComponent} from "./sale/sale.component";
import {AddSaleComponent} from "./sale/add-sale/add-sale.component";
import {PManagerComponent} from "./product/p-manager/p-manager.component";
import {NotFoundComponent} from "./not-found/not-found.component";
import {RoleGuard} from "./guards/guards/role.guard";


const routes: Routes = [
  {path: '', redirectTo: '/landing', pathMatch: 'full'},
  {path: 'landing', component: LandingComponent, canActivate: [NoAuthGuard]},
  {
    path: 'settings', component: SettingsComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: ['admin', 'manager']}
  },
  {path: 'user', component: UserComponent, canActivate: [AuthGuard]},
  {path: 'sign-in', component: SignInComponent, canActivate: [NoAuthGuard]},
  {path: 'sign-up', component: SignUpComponent, canActivate: [NoAuthGuard]},
  {path: 'contact', component: LandingComponent, canActivate: [NoAuthGuard]},
  {path: 'about', component: LandingComponent, canActivate: [NoAuthGuard]},
  {
    path: 'chart', component: ChartComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: ['admin', 'manager']}
  },
  {
    path: 'employees', component: EmployeeComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: ['admin', 'manager']}
  },
  {
    path: 'stores', component: StoreComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: ['admin', 'manager']}
  },
  {
    path: 'add-store', component: AddStoreComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: 'admin'}
  },
  {
    path: 'edit-store/:title', component: AddStoreComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: 'admin'}
  },
  {
    path: 'edit-employee/:email', component: AddEmployeeComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: ['admin', 'manager']}
  },
  {
    path: 'add-employee', component: AddEmployeeComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: ['admin', 'manager']}
  },
  {
    path: 'roles', component: RoleComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: 'admin'}
  },
  {
    path: 'add-role', component: AddRoleComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: 'admin'}
  },
  {
    path: 'edit-role/:roleName', component: AddRoleComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: 'admin'}
  },
  {path: 'cashregisters', component: CashRegisterComponent, canActivate: [AuthGuard]},
  {
    path: 'edit-cashregister/:supermarketName/:registerNumber',
    component: AddCashregisterComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'add-cashregister', component: AddCashregisterComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: ['admin', 'manager']}
  },
  {
    path: 'warehouses', component: WarehouseComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: ['admin', 'manager']}
  },
  {
    path: 'edit-warehouse/:title', component: CashRegisterComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: 'admin'}
  },
  {
    path: 'add-warehouse', component: CashRegisterComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: 'admin'}
  },
  {
    path: 'statuses', component: StatusComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: ['admin', 'manager']}
  },
  {
    path: 'edit-status/:statusName', component: AddStatusComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: 'admin'}
  },
  {
    path: 'add-status', component: AddStatusComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: 'admin'}
  },
  {
    path: 'logs', component: LogsComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: 'admin'}
  },
  {
    path: 'perishable', component: PerishableComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: 'admin'}
  },
  {
    path: 'nonperishable', component: NonperishableComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: 'admin'}
  },
  {
    path: 'catalogue', component: ProductComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: 'admin'}
  },
  {
    path: 'add-product', component: AddProductComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: ['admin', 'manager']}
  },
  {
    path: 'edit-product/:title', component: AddProductComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: ['admin', 'manager']}
  },
  {
    path: 'orders', component: OrderComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: ['admin', 'manager']}
  },
  {
    path: 'add-order', component: AddOrderComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: ['admin', 'manager']}
  },
  {
    path: 'inventory', component: InventoryComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: ['admin', 'manager']}
  },
  {
    path: 'sales', component: SaleComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: ['admin', 'manager']}
  },
  {
    path: 'add-sale', component: AddSaleComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: ['admin', 'manager']}
  },
  {
    path: 'product-manager', component: PManagerComponent,
    canActivate: [AuthGuard, RoleGuard], data: {roles: ['admin', 'manager']}
  },
  {path: '**', component: NotFoundComponent}
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:7234"],
        disallowedRoutes: []
      }
    }),
  ],
  providers: [AuthGuard],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
