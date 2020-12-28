
import { HomeComponent } from './views/Home/home.component';
import { LoginComponent } from './views/Login/login.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [{
  path: "",
  component: LoginComponent
},{
  path: "home",
  component: HomeComponent
}];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
