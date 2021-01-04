import { AuthService } from './../../services/auth.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginForm = this.fb.group({
    username: ['', Validators.required],
    password: ['', Validators.required]
});

  submitted = false;
    busy = false;
    errorMessage = '';

  constructor(private fb: FormBuilder, private router: Router, private auth: AuthService) { }

  ngOnInit(): void {
  }

  login() {
    this.submitted = true;
    this.errorMessage = '';

    if (this.loginForm.invalid) {
        return;
    }

    this.busy = true;

    this.auth.postLogin(this.loginForm.value.username, this.loginForm.value.password).subscribe(() => {
        this.router.navigate(['/home']);
    }, error => {
        this.busy = false;
        this.errorMessage = error.error.error_description;
    });
}

}
