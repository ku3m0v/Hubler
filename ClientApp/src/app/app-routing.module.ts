import {NgModule} from '@angular/core';
import {RouterModule, Routes} from "@angular/router";
import {HomeComponent} from "./home/home.component";
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
import {SpinnerComponent} from "./spinner/spinner.component";
import {AddStoreComponent} from "./store/add-store/add-store.component";
import {NoAuthGuard} from "./guards/no-auth-guard.service";
import {AddEmployeeComponent} from "./employee/add-employee/add-employee.component";
import {RoleComponent} from "./employee/role/role.component";
import {CashRegisterComponent} from "./cashregister/cashregister.component";
import {AddRoleComponent} from "./employee/role/add-role/add-role.component";


const routes: Routes = [
    {path: '', redirectTo: '/landing', pathMatch: 'full'},
    {path: 'landing', component: LandingComponent, canActivate: [NoAuthGuard]},
    {path: 'home', component: HomeComponent, canActivate: [AuthGuard]},
    {path: 'user', component: UserComponent, canActivate: [AuthGuard]},
    {path: 'sign-in', component: SignInComponent},
    {path: 'sign-up', component: SignUpComponent},
    {path: 'contact', component: LandingComponent},
    {path: 'about', component: LandingComponent},
    {path: 'spinner', component: SpinnerComponent},
    {path: 'chart', component: ChartComponent, canActivate: [AuthGuard]},
    {path: 'employees', component: EmployeeComponent, canActivate: [AuthGuard]},
    {path: 'stores', component: StoreComponent, canActivate: [AuthGuard]},
    {path: 'add-store', component: AddStoreComponent, canActivate: [AuthGuard]},
    {path: 'edit-store/:title', component: AddStoreComponent, canActivate: [AuthGuard]},
    {path: 'edit-employee/:id', component: AddEmployeeComponent, canActivate: [AuthGuard]},
    {path: 'add-employee', component: AddEmployeeComponent, canActivate: [AuthGuard]},
    {path: 'roles', component: RoleComponent, canActivate: [AuthGuard]},
    {path: 'add-role', component: AddRoleComponent, canActivate: [AuthGuard]},
    {path: 'edit-role/:roleName', component: AddRoleComponent, canActivate: [AuthGuard]},
    {path: 'cashregisters', component: CashRegisterComponent, canActivate: [AuthGuard]},
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
