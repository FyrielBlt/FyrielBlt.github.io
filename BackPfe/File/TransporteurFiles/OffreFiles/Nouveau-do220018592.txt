<template>
  <div>
    <!-- <Breadcrumb breadcrumb="Profil" /> -->
    <div class="grid grid-flow-col gap-3">
      <div class="row-span-3">
        <!-- <h4 class="text-gray-700">Profil</h4> -->
        <div class="max-w-sm overflow-hidden bg-white rounded shadow-lg">
          <input
            class="w-full mt-2 border-gray-200 rounded-md focus:border-indigo-600 focus:ring focus:ring-opacity-40 focus:ring-indigo-500"
            id="image"
            name="image"
            type="file"
            @change="FileSelected($event)"
            style="display: none"
          />
          <label for="image" class="relative group">
            <img
              style="border-radius: 50px"
              :src="this.image"
              alt="Image Profil"
            />
            <div
              class="opacity-0 group-hover:opacity-70 duration-300 absolute inset-x-0 bottom-0 flex justify-center items-end text-xl bg-black text-white font-semibold"
              style="height: 419px; padding-bottom: 169px"
            >
              Cliquez ici pour changer l'image
            </div>
          </label>
          <div class="px-6 py-4">
            <div class="mb-2 text-xl text-center font-bold text-gray-900">
              {{ "Photo de Profile" }}
            </div>
          </div>
        </div>
      </div>

      <div class="col-span-2">
        <!-- <h4 class="text-gray-600">Modifier :</h4> -->
        <div>
          <div class="p-6 bg-white rounded-md shadow-md">
            <h2 class="text-lg font-semibold text-gray-700 capitalize">
              Paramétres
            </h2>
            <br />
            <div class="grid grid-cols-1 gap-6 mt-4 sm:grid-cols-2">
              <div class="relative block mt-2 sm:mt-0">
                <label class="text-gray-700" for="nom">Nom</label>

                <span class="absolute flex items-center pl-1 py-3">
                  <svg
                    v-if="nom != ''"
                    xmlns="http://www.w3.org/2000/svg"
                    width="16"
                    height="16"
                    fill="currentColor"
                    class="bi bi-check-lg bg-green-500 rounded-r rounded-l"
                    viewBox="0 0 16 16"
                  >
                    <path
                      d="M12.736 3.97a.733.733 0 0 1 1.047 0c.286.289.29.756.01 1.05L7.88 12.01a.733.733 0 0 1-1.065.02L3.217 8.384a.757.757 0 0 1 0-1.06.733.733 0 0 1 1.047 0l3.052 3.093 5.4-6.425a.247.247 0 0 1 .02-.022Z"
                    />
                  </svg>
                  <svg
                    v-else
                    xmlns="http://www.w3.org/2000/svg"
                    width="16"
                    height="16"
                    fill="currentColor"
                    class="bi bi-exclamation-lg bg-red-500 rounded-r rounded-l"
                    viewBox="0 0 16 16"
                  >
                    <path
                      d="M7.005 3.1a1 1 0 1 1 1.99 0l-.388 6.35a.61.61 0 0 1-1.214 0L7.005 3.1ZM7 12a1 1 0 1 1 2 0 1 1 0 0 1-2 0Z"
                    />
                  </svg>
                </span>

                <input
                  id="nom"
                  placeholder="Nom"
                  class="block w-full py-2 pl-8 pr-6 text-xm text-gray-700 placeholder-gray-400 bg-white border border-b border-gray-400 rounded-l rounded-r appearance-none sm:rounded-l-none focus:placeholder-gray-600 focus:text-gray-700 focus:outline-none"
                  :class="[
                    nom === ''
                      ? ' focus:bg-red-100  focus:border-red-800 '
                      : ' focus:bg-green-100  focus:border-green-800 ',
                  ]"
                  v-model="nom"
                />
              </div>
              <div class="relative block mt-2 sm:mt-0">
                <label class="text-gray-700" for="prenom">Prenom</label>

                <span class="absolute flex items-center pl-1 py-3">
                  <svg
                    v-if="prenom != ''"
                    xmlns="http://www.w3.org/2000/svg"
                    width="16"
                    height="16"
                    fill="currentColor"
                    class="bi bi-check-lg bg-green-500 rounded-r rounded-l"
                    viewBox="0 0 16 16"
                  >
                    <path
                      d="M12.736 3.97a.733.733 0 0 1 1.047 0c.286.289.29.756.01 1.05L7.88 12.01a.733.733 0 0 1-1.065.02L3.217 8.384a.757.757 0 0 1 0-1.06.733.733 0 0 1 1.047 0l3.052 3.093 5.4-6.425a.247.247 0 0 1 .02-.022Z"
                    />
                  </svg>
                  <svg
                    v-else
                    xmlns="http://www.w3.org/2000/svg"
                    width="16"
                    height="16"
                    fill="currentColor"
                    class="bi bi-exclamation-lg bg-red-500 rounded-r rounded-l"
                    viewBox="0 0 16 16"
                  >
                    <path
                      d="M7.005 3.1a1 1 0 1 1 1.99 0l-.388 6.35a.61.61 0 0 1-1.214 0L7.005 3.1ZM7 12a1 1 0 1 1 2 0 1 1 0 0 1-2 0Z"
                    />
                  </svg>
                </span>

                <input
                  id="prenom"
                  placeholder="Prenom"
                  class="block w-full py-2 pl-8 pr-6 text-xm text-gray-700 placeholder-gray-400 bg-white border border-b border-gray-400 rounded-l rounded-r appearance-none sm:rounded-l-none focus:placeholder-gray-600 focus:text-gray-700 focus:outline-none"
                  :class="[
                    prenom === ''
                      ? ' focus:bg-red-100  focus:border-red-800 '
                      : ' focus:bg-green-100  focus:border-green-800 ',
                  ]"
                  v-model="prenom"
                />
              </div>
              <div class="relative block mt-2 sm:mt-0">
                <label class="text-gray-700" for="email">Email</label>

                <span class="absolute flex items-center pl-1 py-3">
                  <svg
                    v-if="email != ''"
                    xmlns="http://www.w3.org/2000/svg"
                    width="16"
                    height="16"
                    fill="currentColor"
                    class="bi bi-check-lg bg-green-500 rounded-r rounded-l"
                    viewBox="0 0 16 16"
                  >
                    <path
                      d="M12.736 3.97a.733.733 0 0 1 1.047 0c.286.289.29.756.01 1.05L7.88 12.01a.733.733 0 0 1-1.065.02L3.217 8.384a.757.757 0 0 1 0-1.06.733.733 0 0 1 1.047 0l3.052 3.093 5.4-6.425a.247.247 0 0 1 .02-.022Z"
                    />
                  </svg>
                  <svg
                    v-else
                    xmlns="http://www.w3.org/2000/svg"
                    width="16"
                    height="16"
                    fill="currentColor"
                    class="bi bi-exclamation-lg bg-red-500 rounded-r rounded-l"
                    viewBox="0 0 16 16"
                  >
                    <path
                      d="M7.005 3.1a1 1 0 1 1 1.99 0l-.388 6.35a.61.61 0 0 1-1.214 0L7.005 3.1ZM7 12a1 1 0 1 1 2 0 1 1 0 0 1-2 0Z"
                    />
                  </svg>
                </span>

                <input
                  id="email"
                  placeholder="Email"
                  class="block w-full py-2 pl-8 pr-6 text-xm text-gray-700 placeholder-gray-400 bg-white border border-b border-gray-400 rounded-l rounded-r appearance-none sm:rounded-l-none focus:placeholder-gray-600 focus:text-gray-700 focus:outline-none"
                  :class="[
                    email === ''
                      ? ' focus:bg-red-100  focus:border-red-800 '
                      : ' focus:bg-green-100  focus:border-green-800 ',
                  ]"
                  v-model="email"
                />
              </div>
              <div class="relative block mt-2 sm:mt-0">
                <label class="text-gray-700" for="tel">Tel</label>
                <span class="absolute flex items-center pl-1 py-3">
                  <svg
                    v-if="tel != ''"
                    xmlns="http://www.w3.org/2000/svg"
                    width="16"
                    height="16"
                    fill="currentColor"
                    class="bi bi-check-lg bg-green-500 rounded-r rounded-l"
                    viewBox="0 0 16 16"
                  >
                    <path
                      d="M12.736 3.97a.733.733 0 0 1 1.047 0c.286.289.29.756.01 1.05L7.88 12.01a.733.733 0 0 1-1.065.02L3.217 8.384a.757.757 0 0 1 0-1.06.733.733 0 0 1 1.047 0l3.052 3.093 5.4-6.425a.247.247 0 0 1 .02-.022Z"
                    />
                  </svg>
                  <svg
                    v-else
                    xmlns="http://www.w3.org/2000/svg"
                    width="16"
                    height="16"
                    fill="currentColor"
                    class="bi bi-exclamation-lg bg-red-500 rounded-r rounded-l"
                    viewBox="0 0 16 16"
                  >
                    <path
                      d="M7.005 3.1a1 1 0 1 1 1.99 0l-.388 6.35a.61.61 0 0 1-1.214 0L7.005 3.1ZM7 12a1 1 0 1 1 2 0 1 1 0 0 1-2 0Z"
                    />
                  </svg>
                </span>
                <input
                  id="tel"
                  placeholder="Téléphone"
                  class="block w-full py-2 pl-8 pr-6 text-xm text-gray-700 placeholder-gray-400 bg-white border border-b border-gray-400 rounded-l rounded-r appearance-none sm:rounded-l-none focus:placeholder-gray-600 focus:text-gray-700 focus:outline-none"
                  :class="[
                    tel === ''
                      ? ' focus:bg-red-100  focus:border-red-800 '
                      : ' focus:bg-green-100  focus:border-green-800 ',
                  ]"
                  v-model="tel"
                />
              </div>

              <div class="relative block mt-2 sm:mt-0">
                <label class="text-gray -700" for="motdepasse"
                  >Mot de passe</label
                >

                <span class="absolute flex items-center pl-1 py-3">
                  <svg
                    v-if="password != ''"
                    xmlns="http://www.w3.org/2000/svg"
                    width="16"
                    height="16"
                    fill="currentColor"
                    class="bi bi-check-lg bg-green-500 rounded-r rounded-l"
                    viewBox="0 0 16 16"
                  >
                    <path
                      d="M12.736 3.97a.733.733 0 0 1 1.047 0c.286.289.29.756.01 1.05L7.88 12.01a.733.733 0 0 1-1.065.02L3.217 8.384a.757.757 0 0 1 0-1.06.733.733 0 0 1 1.047 0l3.052 3.093 5.4-6.425a.247.247 0 0 1 .02-.022Z"
                    />
                  </svg>
                  <svg
                    v-else
                    xmlns="http://www.w3.org/2000/svg"
                    width="16"
                    height="16"
                    fill="currentColor"
                    class="bi bi-exclamation-lg bg-red-500 rounded-r rounded-l"
                    viewBox="0 0 16 16"
                  >
                    <path
                      d="M7.005 3.1a1 1 0 1 1 1.99 0l-.388 6.35a.61.61 0 0 1-1.214 0L7.005 3.1ZM7 12a1 1 0 1 1 2 0 1 1 0 0 1-2 0Z"
                    />
                  </svg>
                </span>

                <input
                  type="password"
                  id="motdepasse"
                  placeholder="password"
                  class="block w-full py-2 pl-8 pr-6 text-xm text-gray-700 placeholder-gray-400 bg-white border border-b border-gray-400 rounded-l rounded-r appearance-none sm:rounded-l-none focus:placeholder-gray-600 focus:text-gray-700 focus:outline-none"
                  :class="[
                    password === ''
                      ? ' focus:bg-red-100  focus:border-red-800 '
                      : ' focus:bg-green-100  focus:border-green-800 ',
                  ]"
                  v-model="password"
                />
              </div>
              <div class="relative block mt-2 sm:mt-0">
                <label class="text-gray-700" for="cmotdepasse"
                  >Confirmer mot de passe</label
                >

                <span class="absolute flex items-center pl-1 py-3">
                  <svg
                    v-if="cpassword != '' && cpassword == password"
                    xmlns="http://www.w3.org/2000/svg"
                    width="16"
                    height="16"
                    fill="currentColor"
                    class="bi bi-check-lg bg-green-500 rounded-r rounded-l"
                    viewBox="0 0 16 16"
                  >
                    <path
                      d="M12.736 3.97a.733.733 0 0 1 1.047 0c.286.289.29.756.01 1.05L7.88 12.01a.733.733 0 0 1-1.065.02L3.217 8.384a.757.757 0 0 1 0-1.06.733.733 0 0 1 1.047 0l3.052 3.093 5.4-6.425a.247.247 0 0 1 .02-.022Z"
                    />
                  </svg>
                  <svg
                    v-else
                    xmlns="http://www.w3.org/2000/svg"
                    width="16"
                    height="16"
                    fill="currentColor"
                    class="bi bi-exclamation-lg bg-red-500 rounded-r rounded-l"
                    viewBox="0 0 16 16"
                  >
                    <path
                      d="M7.005 3.1a1 1 0 1 1 1.99 0l-.388 6.35a.61.61 0 0 1-1.214 0L7.005 3.1ZM7 12a1 1 0 1 1 2 0 1 1 0 0 1-2 0Z"
                    />
                  </svg>
                </span>

                <input
                  type="password"
                  placeholder="Confirmer password"
                  class="block w-full py-2 pl-8 pr-6 text-xm text-gray-700 placeholder-gray-400 bg-white border border-b border-gray-400 rounded-l rounded-r appearance-none sm:rounded-l-none focus:placeholder-gray-600 focus:text-gray-700 focus:outline-none"
                  :class="[cpassword === '' ? class1 : class2]"
                  v-model="cpassword"
                />
              </div>
            </div>
            <div class="flex justify-end mt-4">
              <button
                class="px-4 py-2 text-gray-200 bg-gray-800 rounded-md hover:bg-gray-700 focus:outline-none focus:bg-gray-700"
                @click="Modifier()"
              >
                Modifier
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
    <br />
  </div>
</template>

<script>
import axios from "axios";
import BreadCrumb from "/src/components/Transporteur/ProfilBridal.vue";
export default {
  components: {
    BreadCrumb,
  },
  data() {
    return {
      Profil: [],
      nom: "",
      reslt:false,
      prenom: "",
      email: "",
      tel: "",
      motdepasse: "",
      password: "",
      cpassword: "",
      user: "",
      class1: "",
      class2: "",
      idUser: "",
      image: "",
      imageFile: "",
    };
  },
  async created() {
    await axios
      .get("http://localhost:5000/api/users/" + localStorage.getItem("iduser"))
      .then((res) => {
        this.Profil = res.data;
        this.image = res.data.imageSrc;
        this.user = res.data;
        this.nom = res.data.nom;
        this.prenom = res.data.prenom;
        this.email = res.data.email;
        this.idUser = res.data.idUser;
        (this.motdepasse = res.data.motdepasse), (this.tel = res.data.tel);
      });
  },
  methods: {
    // ajouter image
    FileSelected(event) {
      this.imageFile = event.target.files[0];
     
        this.image=URL.createObjectURL(this.imageFile)
      
     
    },
    Modifier() {
      Swal.fire({
        title: "Entrer Ancien mot de passe pour continuer",
        input: "password",
        inputAttributes: {
          autocapitalize: "off",
        },
        showCancelButton: true,
        confirmButtonText: "Modifier",
        showLoaderOnConfirm: true,
        preConfirm: (login) => {
          if (login == this.motdepasse) {
            if (this.password != "") {
              if (
                this.password == this.cpassword ||
                this.password == this.motdepasse
              ) {
                let user = new FormData();
                user.append("idUser", this.idUser);
                user.append("Nom", this.nom);
                user.append("Prenom", this.prenom);
                user.append("Email", this.email);
                user.append("societe", localStorage.getItem("societe"));
                user.append("motdepasse", this.password);
                user.append("image", this.Profil.image);
                user.append("ImageFile", this.imageFile);
                user.append("tel", this.tel);
                user.append("ImageSrc", "");
                axios
                  .put(
                    "http://localhost:5000/api/Users/" + this.idUser,
                    user
                  )
                  .then((res) => {
                    this.user = res.data;
                    this.nom = res.data.nom;
                    this.prenom = res.data.prenom;
                    this.email = res.data.email;
                    this.image = res.data.imageSrc;
                    this.imageFile = "";
                    this.password = "";
                    this.cpassword = "";
                    this.tel = res.data.tel;
                     this.$swal({
            position: "top-end",
            icon: "success",
            toast: true,
            title: "Profil Modifié",
            showConfirmButton: false,
            timer: 2000,
          });
                  }).catch(()=>{
           this.$swal({
          position: "top-end",
          icon: "error",
          toast: true,
          title: "Un compte utilisateur déja créer avec cette adress mail",
          showConfirmButton: false,
          timer: 2000,
        });
                  })
                 
              }
               if (
          this.password != this.cpassword &&
          this.password != this.motdepasse
        ) {
          this.$swal({
            position: "top-end",
            icon: "error",
            toast: true,
            title: "Confirmez mot de passe!",
            showConfirmButton: false,
            timer: 2000,
          });
            }
         
          }
            else if (this.password == "") {
              let user = new FormData();
              user.append("idUser", this.idUser);
              user.append("Nom", this.nom);
              user.append("Prenom", this.prenom);
              user.append("Email", this.email);
              user.append("societe", localStorage.getItem("societe"));
              user.append("motdepasse", this.motdepasse);
              user.append("image", this.Profil.image);
              user.append("tel", this.tel);
              user.append("ImageFile", this.imageFile);
              user.append("ImageSrc", "");
              axios
                .put("http://localhost:5000/api/Users/" + this.idUser, user)
                .then((res) => {
                  console.log(res.data);
                  this.user = res.data;
                  this.nom = res.data.nom;
                  this.prenom = res.data.prenom;
                  this.email = res.data.email;
                  this.image = res.data.imageSrc;
                  this.imageFile = "";
                  this.password = "";
                  this.cpassword = "";
                  this.tel = res.data.tel;
                   this.$swal({
            position: "top-end",
            icon: "success",
            toast: true,
            title: "Profil Modifié",
            showConfirmButton: false,
            timer: 2000,
          });
                 
                }).catch(()=>{
                    this.$swal({
          position: "top-end",
          icon: "error",
          toast: true,
          title: "Un compte utilisateur déja créer avec cette adress mail",
          showConfirmButton: false,
          timer: 2000,
        });
                })
                
            }
      
       // allowOutsideClick: () => !Swal.isLoading(),
          }
          else if (login!=this.motdepasse) {
          this.$swal({
          position: "top-end",
          icon: "error",
          toast: true,
          title: "Ancien mot de passe incorrecte",
          showConfirmButton: false,
          timer: 2000,
        });
          }
      }
      })}
  }
  ,
  watch: {
    cpassword() {
      if (this.password != "" && this.cpassword == "") {
        this.cla = "form-control";
      } else if (this.password != "" && this.cpassword != "") {
        if (this.password == this.cpassword) {
          this.class1 = "focus:bg-green-200  focus:border-green-800 ";
          this.class2 = "focus:bg-green-200  focus:border-green-800 ";
        } else {
          this.class1 = "form-control";
          this.class2 = "focus:bg-red-100  focus:border-red-800 ";
        }
      }
    },
  }
    
};
</script>