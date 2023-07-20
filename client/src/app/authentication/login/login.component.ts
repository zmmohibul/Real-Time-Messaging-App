import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginModel } from '../../models/authentication/loginModel';
import { User } from '../../models/authentication/user';
import { AuthenticationService } from '../../services/authentication.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
  loginForm: FormGroup = new FormGroup({});
  model: LoginModel = { userName: '', password: '' };
  loading = false;

  constructor(
    public authService: AuthenticationService,
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      userName: ['', [Validators.required]],
      password: ['', [Validators.required]],
    });
  }

  login() {
    if (this.loginForm.invalid) {
      return;
    }

    this.loading = true;
    this.model = { ...this.loginForm.value };
    this.authService.login(this.model).subscribe({
      next: (user: User) => {
        console.log(user);
        this.toastr.success(`${user.userName}`, `Welcome`);
        this.loading = false;
        this.router.navigateByUrl('/messages');
      },
      error: (err) => {
        this.toastr.error(err.error.errorMessage);
        this.loading = false;
      },
    });
  }
}
