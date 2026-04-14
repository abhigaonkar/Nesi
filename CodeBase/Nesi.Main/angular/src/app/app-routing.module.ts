import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { HomeComponent } from './pages/home/home.component';
import { SignInComponent } from './pages/signin/signin.component';
import { Nesi1Component } from './pages/Nesi1/Nesi1.component';
import { SignOutComponent } from './pages/signout/signout.component';
import { AuthGuard } from './services/authentication/authGuard';
import { FvrComponent } from './pages/fvr/fvr.component';
import { OpensComponent } from './opens/opens.component';

@NgModule({
  imports: [
    RouterModule.forRoot([
      { path: '', redirectTo: '/signin', pathMatch: 'full' },
      { path: 'signin', component: SignInComponent, data: { title: 'Spark Ops - Sign In' } },
      { path: 'fvrs', component: FvrComponent, data: { title: 'Spark Ops - FVRs Check' } },
      { path: 'signout', component: SignOutComponent, data: { title: 'Spark Ops Sign Out' }  },
      { path: 'home', loadChildren: './home/home.module#HomeModule' },
      { path: 'opens', loadChildren: './opens/opens.module#OpensModule' },
    ],
      { enableTracing: false } // <-- debugging purposes only
    )
  ],
  exports: [RouterModule]
})

export class AppRoutingModule { }
