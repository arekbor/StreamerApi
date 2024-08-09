import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { StreamerService } from "./streamer.service";

import { ImageModule } from "primeng/image";
import { PaginatorModule } from "primeng/paginator";
import { TableModule } from "primeng/table";

import { AvatarModule } from "primeng/avatar";
import { AvatarGroupModule } from "primeng/avatargroup";

import { provideHttpClient, withFetch, withInterceptorsFromDi } from "@angular/common/http";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    TableModule,
    PaginatorModule,
    ImageModule,
    AvatarModule,
    AvatarGroupModule,
  ],
  providers: [provideHttpClient(withInterceptorsFromDi()), StreamerService],
  bootstrap: [AppComponent],
})
export class AppModule {}
