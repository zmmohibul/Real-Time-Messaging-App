import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginModel } from '../../models/authentication/loginModel';
import { AuthenticationService } from '../../services/authentication.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { User } from '../../models/authentication/user';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent {
  registerForm: FormGroup = new FormGroup({});
  model: LoginModel = { userName: '', password: '' };
  loading = false;

  constructor(
    public authService: AuthenticationService,
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      userName: ['', [Validators.required]],
      password: ['', [Validators.required]],
      confirmPassword: ['', [Validators.required]],
    });
  }

  register() {
    if (this.registerForm.invalid) {
      return;
    }

    this.loading = true;
    this.model = { ...this.registerForm.value };
    this.authService.register(this.model).subscribe({
      next: (user: User) => {
        console.log(user);
        this.toastr.success(`${user.userName}`, `Account Created`);
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
