# UserPlatform
API que simula una plataforma de usuarios con registro y login para dos tipos de roles (Administrador y Operativo), cada uno con las siguientes funcionalidades¡
- Rol Administrador: Puede crear nuevos usuarios, consultar, editar y borrar usuarios.
- Rol Operativo: Puede consul¤ar los usuarios que creó el perfil adminis¤rador.
 
El proyecto cuenta con dos capas; la capa de datos llamada "UserPlatform.Domain" (donde estan las entidades, los modelos de datos, los enum, la carpeta de migraciones y el contexto de datos, que me permite conectarme a la BD), tambien esta la capa "UserPlatform.API" en la cual se encuentra toda la logica de la aplicación, los contralodores, helpers y demás clases requeridas para dar respuesta a este requerimiento.
